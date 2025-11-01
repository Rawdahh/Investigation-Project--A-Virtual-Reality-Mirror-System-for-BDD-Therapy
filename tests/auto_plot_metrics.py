import pandas as pd
import matplotlib.pyplot as plt
import os

# --- Configuration ---
# Define the names of your four CSV files
CSV_FILES = [
    'NormalGameRun1.csv',
    'GameRun2 (5 Restaurants).csv',
    'GameRun3(25 Restaurants).csv', # Assuming this is the correct name for the 3rd file snippet
    'GameRun4(15 Restaurants).csv'  # Assuming this is the correct name for the 4th file snippet
]

# Columns that should be excluded from automatic plotting (IDs, indices, state codes)
EXCLUDE_COLS = [
    'Time Stamp', 
    # 'Time (Seconds)' exists only in NormalGameRun1. We handle its removal/conversion below.
    'power_level_state',
    'foveation_level',
    'spacewarp_motion_vector_type',
    # Note: We keep "display_refresh_rate" as it is a numerical metric of interest.
]

# --- Helper Function for Data Preparation ---

def load_and_merge_data(file_list):
    """
    Loads a list of semicolon-delimited CSV files, handles comma decimals, 
    adds a source identifier, and merges them.
    """
    all_data = []

    print("Loading files...")
    for file_path in file_list:
        try:
            if not os.path.exists(file_path):
                print(f"Warning: File not found: {file_path}. Skipping.")
                continue
                
            # 1. Read CSV using the correct delimiter (;) and decimal separator (,)
            df = pd.read_csv(
                file_path,
                delimiter=';',
                decimal=','
            )
            
            # 2. Clean up column names (strip whitespace and handle potential invisible BOM character)
            df.columns = df.columns.str.strip()
            # Special handling for BOM character in the first column name if it exists
            if df.columns[0].startswith('\ufeff'):
                df.columns.values[0] = df.columns[0].lstrip('\ufeff').strip()
            
            # 3. Handle the inconsistent 'Time (Seconds)' column.
            # Convert it to numeric if possible, otherwise drop it if it exists.
            if 'Time (Seconds)' in df.columns:
                # We try to convert it to numeric (it should be floats after decimal=',' fix)
                # If we plan to use the index/Time Stamp for X-axis across all files, 
                # we drop the seconds column to keep the comparison matrix consistent.
                df = df.drop(columns=['Time (Seconds)'])
                
            # 4. Add a column to identify the source file
            df['Source_File'] = os.path.basename(file_path)
            all_data.append(df)
            print(f"Successfully loaded {file_path}")

        except Exception as e:
            print(f"Error loading {file_path}: {e}")

    if not all_data:
        raise ValueError("No valid CSV files were loaded.")

    # 5. Ensure all dataframes have consistent numerical types where possible
    merged_df = pd.concat(all_data, ignore_index=True)
    
    # Force conversion of all numerical columns to a single robust numeric type (float)
    for col in merged_df.columns:
        if col not in ['Source_File'] and merged_df[col].dtype != object:
             merged_df[col] = pd.to_numeric(merged_df[col], errors='coerce')
    
    return merged_df

# --- Helper Function for Plotting ---

def plot_metric_comparison(df, metric_column):
    """Generates a line plot comparing a single metric across all source files."""
    
    # Ensure the metric column exists
    if metric_column not in df.columns:
        return

    plt.figure(figsize=(14, 7))
    
    # Get unique source files for plotting loops
    sources = df['Source_File'].unique()
    
    # Determine the index (X-axis). Using 'Time Stamp' for consistency if available.
    x_axis_col = 'Time Stamp' if 'Time Stamp' in df.columns else df.index

    for source in sources:
        # Filter the data for the current source file
        source_data = df[df['Source_File'] == source].reset_index()
        
        # Determine X-axis values for this specific source
        x_values = source_data[x_axis_col] if isinstance(x_axis_col, str) else source_data.index
        
        # Plot the metric. 
        plt.plot(x_values, source_data[metric_column], 
                 label=f'{source}', 
                 alpha=0.8) 

    plt.title(f'Comparison of {metric_column} Across Data Sets', fontsize=16)
    
    # Adjust X-axis label based on what was used
    x_label = x_axis_col if isinstance(x_axis_col, str) else 'Data Point Index (Sequential)'
    plt.xlabel(x_label, fontsize=12)
    
    plt.ylabel(metric_column, fontsize=12)
    plt.legend(title='File Source')
    plt.grid(True, linestyle='--', alpha=0.6)
    plt.tight_layout()
    # We use plt.figure(figsize) and then plt.show() inside the loop. 
    # This automatically generates separate windows for each plot.
    plt.show()

# --- Main Execution ---

if __name__ == "__main__":
    try:
        # 1. Load and merge the data
        merged_data = load_and_merge_data(CSV_FILES)

        # 2. Identify all numerical metrics suitable for plotting
        # Select only numerical columns (integers and floats)
        numerical_cols = merged_data.select_dtypes(include=['number']).columns.tolist()

        # Filter out exclusion columns
        metrics_to_plot = [
            col for col in numerical_cols 
            if col not in EXCLUDE_COLS
        ]

        if not metrics_to_plot:
            print("Error: No suitable numerical metrics found after filtering.")
        else:
            print(f"\n--- Generating {len(metrics_to_plot)} Comparison Plots ---")
            
            # 3. Loop through all identified metrics and plot each one
            for metric in metrics_to_plot:
                print(f"Plotting: {metric}")
                plot_metric_comparison(merged_data, metric)
                
            print("\nAll plots generated successfully.")

    except ValueError as e:
        print(f"\nFATAL ERROR: {e}")
    except Exception as e:
        print(f"\nAn unexpected error occurred: {e}")