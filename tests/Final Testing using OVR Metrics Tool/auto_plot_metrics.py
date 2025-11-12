import pandas as pd
import matplotlib.pyplot as plt
import os

# --- Configuration ---
CSV_FILES = [
    'NormalGameRun1.csv',
    'GameRun2 (5 Restaurants).csv',
    'GameRun3(25 Restaurants).csv',
    'GameRun4(15 Restaurants).csv'
]

# Metrics for which we need to calculate the average
AVERAGE_METRICS = [
    'average_frame_rate',
    'gpu_utilization_percentage',
    'battery_temperature_celcius'
]

# --- Helper Function for Data Preparation (Unchanged) ---

def load_and_merge_data(file_list):
    """Loads CSV data, converts 'Time Stamp' (ms) to 'Time (Minutes)', and merges."""
    all_data = []

    for file_path in file_list:
        try:
            if not os.path.exists(file_path):
                continue
                
            df = pd.read_csv(
                file_path,
                delimiter=';',
                decimal=','
            )
            
            df.columns = df.columns.str.strip()
            if df.columns[0].startswith('\ufeff'):
                df.columns.values[0] = df.columns[0].lstrip('\ufeff').strip()
            
            if 'Time (Seconds)' in df.columns:
                df = df.drop(columns=['Time (Seconds)'])
                
            df['Time Stamp'] = pd.to_numeric(df['Time Stamp'], errors='coerce')
            df['Time (Minutes)'] = df['Time Stamp'] / 1000 / 60
            
            df['Source_File'] = os.path.basename(file_path)
            all_data.append(df)

        except Exception as e:
            print(f"Error loading {file_path}: {e}")
            pass 

    if not all_data:
        raise ValueError("No valid CSV files were loaded.")

    merged_df = pd.concat(all_data, ignore_index=True)
    
    for col in merged_df.columns:
        if col not in ['Source_File', 'Time (Minutes)', 'Time Stamp'] and merged_df[col].dtype != object:
             merged_df[col] = pd.to_numeric(merged_df[col], errors='coerce')
    
    return merged_df

# --- Calculation Function ---

def calculate_and_print_averages(df, metrics):
    """Calculates the mean of specified metrics, grouped by Source_File, and prints."""
    
    print("\n" + "="*50)
    print("--- AVERAGE METRICS PER GAME RUN ---")
    print("="*50)
    
    # 1. Select only the necessary columns (Source File + metrics to average)
    calc_df = df[['Source_File'] + [m for m in metrics if m in df.columns]]

    # 2. Group the data by source file and calculate the mean for each metric
    average_results = calc_df.groupby('Source_File')[metrics].mean()
    
    # --- Formatting Output ---
    
    # A mapping for clean console labels (optional, but helpful)
    LEGEND_MAPPING = {
        'NormalGameRun1.csv': 'Game Run 1 (Normal)',
        'GameRun2 (5 Restaurants).csv': 'Game Run 2 (5 Restaurants)',
        'GameRun3(25 Restaurants).csv': 'Game Run 3 (25 Restaurants)',
        'GameRun4(15 Restaurants).csv': 'Game Run 4 (15 Restaurants)',
    }
    
    # Rename index for better display
    average_results.index = average_results.index.map(lambda x: LEGEND_MAPPING.get(x, x))
    
    # Rename columns for clarity
    average_results.rename(columns={
        'average_frame_rate': 'Average FPS',
        'gpu_utilization_percentage': 'Avg GPU Util (%)',
        'battery_temperature_celcius': 'Avg Battery Temp (°C)'
    }, inplace=True)
    
    # Print the resulting table to the console
    print(average_results.to_string(float_format="%.2f"))
    print("="*50)

# --- Main Execution ---

if __name__ == "__main__":
    print("Loading files...")
    
    try:
        merged_data = load_and_merge_data(CSV_FILES)

        # ----------------------------------------------------
        # 1. CALCULATE AND PRINT AVERAGES
        # ----------------------------------------------------
        calculate_and_print_averages(merged_data, AVERAGE_METRICS)

        
    except ValueError as e:
        print(f"\nFATAL ERROR: {e}")
    except Exception as e:
        print(f"\nAn unexpected error occurred: {e}")