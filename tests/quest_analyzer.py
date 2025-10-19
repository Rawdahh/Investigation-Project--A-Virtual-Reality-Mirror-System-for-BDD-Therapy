#  log_file_path = ".\QuestPerformanceLog_2025-10-19_15-45-15[1].txt"

import re
import matplotlib.pyplot as plt
import pandas as pd
from datetime import datetime
import numpy as np

def parse_quest_log(file_path):
    """Parse the Quest performance log file and extract metrics."""
    
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
    except FileNotFoundError:
        print(f"Error: The file '{file_path}' was not found.")
        return None, None
    
    # Extract session info
    session_info = {}
    session_info['start_time'] = re.search(r'Session Start Time: (.+)', content)
    session_info['device'] = re.search(r'Device Model: (.+)', content)
    session_info['os'] = re.search(r'Operating System: (.+)', content)
    session_info['gpu'] = re.search(r'GPU: (.+)', content)
    session_info['refresh_rate'] = re.search(r'Display Refresh Rate: ([\d.]+)', content)
    
    # Extract all metrics from log lines
    pattern = r'Time: (\d{2}:\d{2}:\d{2}), Battery: (\d+)%, Temp: ([\d.]+)°C, CPU: (\d+), GPU: (\d+), MemAlloc: (\d+)MB, MemReserved: (\d+)MB, Scene: ([^,]+), FPS: ([\d.]+), FrameSpikes: (\d+)'
    
    matches = re.findall(pattern, content)
    
    if not matches:
        print("No data found in the log file! Please check the log format.")
        return None, None
    
    # Create DataFrame
    data = {
        'Time': [m[0] for m in matches],
        'Battery': [int(m[1]) for m in matches],
        'Temperature': [float(m[2]) for m in matches],
        'CPU_Level': [int(m[3]) for m in matches],
        'GPU_Level': [int(m[4]) for m in matches],
        'Memory_Allocated': [int(m[5]) for m in matches],
        'Memory_Reserved': [int(m[6]) for m in matches],
        'Scene': [m[7] for m in matches],
        'FPS': [float(m[8]) for m in matches],
        'Frame_Spikes': [int(m[9]) for m in matches]
    }
    
    df = pd.DataFrame(data)
    
    # Convert time to seconds from start for easier plotting
    times = pd.to_datetime(df['Time'], format='%H:%M:%S')
    df['Seconds'] = (times - times.iloc[0]).dt.total_seconds()
    
    return df, session_info

def create_fps_analysis(df, session_info, output_path='figure1_fps_analysis.png'):
    """Figure 1: FPS Performance Analysis."""
    
    fig, axes = plt.subplots(2, 3, figsize=(18, 10))
    fig.suptitle('Figure 1: FPS Performance Analysis', fontsize=16, fontweight='bold')
    
    # Session info at top-left
    info_text = f"Device: {session_info['device'].group(1) if session_info['device'] else 'N/A'} | "
    info_text += f"GPU: {session_info['gpu'].group(1) if session_info['gpu'] else 'N/A'} | "
    info_text += f"Refresh Rate: {session_info['refresh_rate'].group(1) if session_info['refresh_rate'] else 'N/A'} Hz"
    fig.text(0.01, 0.98, info_text, fontsize=9, ha='left', 
             bbox=dict(boxstyle='round', facecolor='wheat', alpha=0.3))
    
    x = df['Seconds'].values
    
    # 1. FPS Over Time
    ax = axes[0, 0]
    ax.plot(x, df['FPS'], color='#2ecc71', linewidth=2, marker='o', markersize=4)
    ax.axhline(y=72, color='r', linestyle='--', label='Target (72 FPS)', alpha=0.7)
    ax.fill_between(x, df['FPS'], alpha=0.3, color='#2ecc71')
    ax.set_title('FPS Performance', fontweight='bold')
    ax.set_xlabel('Time (seconds)')
    ax.set_ylabel('FPS')
    ax.legend()
    ax.grid(True, alpha=0.3)
    
    # 2. FPS Distribution (Histogram)
    ax = axes[0, 1]
    ax.hist(df['FPS'], bins=15, color='#2ecc71', alpha=0.7, edgecolor='black')
    ax.axvline(df['FPS'].mean(), color='r', linestyle='--', linewidth=2, label=f'Mean: {df["FPS"].mean():.1f}')
    ax.set_title('FPS Distribution', fontweight='bold')
    ax.set_xlabel('FPS')
    ax.set_ylabel('Frequency')
    ax.legend()
    ax.grid(True, alpha=0.3, axis='y')
    
    # 3. FPS vs Temperature
    ax = axes[0, 2]
    scatter = ax.scatter(df['Temperature'], df['FPS'], c=df['Frame_Spikes'], 
                        cmap='RdYlGn_r', s=100, alpha=0.6, edgecolors='black')
    ax.set_title('FPS vs Temperature', fontweight='bold')
    ax.set_xlabel('Temperature (°C)')
    ax.set_ylabel('FPS')
    cbar = plt.colorbar(scatter, ax=ax)
    cbar.set_label('Frame Spikes')
    ax.grid(True, alpha=0.3)
    
    # 4. Frame Spikes
    ax = axes[1, 0]
    colors = ['#e74c3c' if s > 10 else '#f39c12' if s > 5 else '#3498db' for s in df['Frame_Spikes']]
    ax.bar(x, df['Frame_Spikes'], color=colors, alpha=0.7)
    ax.set_title('Frame Spikes Count', fontweight='bold')
    ax.set_xlabel('Time (seconds)')
    ax.set_ylabel('Spike Count')
    ax.grid(True, alpha=0.3, axis='y')
    
    # 5. Hide the subplot where the statistics text used to be.
    axes[1, 1].axis('off')
    
    # 6. Hide the last subplot.
    axes[1, 2].axis('off')
    
    plt.tight_layout(rect=[0, 0, 1, 0.96])
    plt.savefig(output_path, dpi=300, bbox_inches='tight')
    print(f"Figure 1 saved to: {output_path}")
    plt.close()

def create_cpu_gpu_analysis(df, output_path='figure2_cpu_gpu_analysis.png'):
    """Figure 2: CPU and GPU Performance Levels."""
    
    fig, axes = plt.subplots(1, 2, figsize=(14, 5))
    fig.suptitle('Figure 2: CPU & GPU Performance Levels', fontsize=16, fontweight='bold')
    
    x = df['Seconds'].values
    
    # 1. CPU Level
    ax = axes[0]
    ax.plot(x, df['CPU_Level'], color='#3498db', linewidth=2.5, marker='o', markersize=6)
    ax.fill_between(x, df['CPU_Level'], alpha=0.3, color='#3498db')
    ax.set_title('CPU Performance Level', fontweight='bold', fontsize=14)
    ax.set_xlabel('Time (seconds)', fontsize=12)
    ax.set_ylabel('CPU Level', fontsize=12)
    ax.grid(True, alpha=0.3)
    
    # Add statistics
    avg_cpu = df['CPU_Level'].mean()
    ax.axhline(y=avg_cpu, color='r', linestyle='--', linewidth=2, label=f'Average: {avg_cpu:.2f}', alpha=0.7)
    ax.legend(fontsize=10)
    
    # 2. GPU Level
    ax = axes[1]
    ax.plot(x, df['GPU_Level'], color='#e74c3c', linewidth=2.5, marker='s', markersize=6)
    ax.fill_between(x, df['GPU_Level'], alpha=0.3, color='#e74c3c')
    ax.set_title('GPU Performance Level', fontweight='bold', fontsize=14)
    ax.set_xlabel('Time (seconds)', fontsize=12)
    ax.set_ylabel('GPU Level', fontsize=12)
    ax.grid(True, alpha=0.3)
    
    # Add statistics
    avg_gpu = df['GPU_Level'].mean()
    ax.axhline(y=avg_gpu, color='b', linestyle='--', linewidth=2, label=f'Average: {avg_gpu:.2f}', alpha=0.7)
    ax.legend(fontsize=10)
    
    plt.tight_layout(rect=[0, 0, 1, 0.96])
    plt.savefig(output_path, dpi=300, bbox_inches='tight')
    print(f"Figure 2 saved to: {output_path}")
    plt.close()

def create_memory_analysis(df, output_path='figure3_memory_analysis.png'):
    """Figure 3: Memory Usage Analysis."""
    
    fig, ax = plt.subplots(1, 1, figsize=(12, 6))
    fig.suptitle('Figure 3: Memory Usage Analysis', fontsize=16, fontweight='bold')
    
    x = df['Seconds'].values
    
    # Memory plot
    ax.plot(x, df['Memory_Allocated'], label='Allocated', color='#1abc9c', 
            linewidth=2.5, marker='o', markersize=6)
    ax.plot(x, df['Memory_Reserved'], label='Reserved', color='#34495e', 
            linewidth=2.5, marker='s', markersize=6)
    ax.fill_between(x, df['Memory_Allocated'], alpha=0.2, color='#1abc9c')
    ax.fill_between(x, df['Memory_Reserved'], alpha=0.2, color='#34495e')
    
    ax.set_title('Memory Allocation Over Time', fontweight='bold', fontsize=14)
    ax.set_xlabel('Time (seconds)', fontsize=12)
    ax.set_ylabel('Memory (MB)', fontsize=12)
    ax.legend(fontsize=12, loc='best')
    ax.grid(True, alpha=0.3)
    
    # Add memory efficiency text
    avg_allocated = df['Memory_Allocated'].mean()
    avg_reserved = df['Memory_Reserved'].mean()
    efficiency = (avg_allocated / avg_reserved) * 100 if avg_reserved > 0 else 0
    
    info_text = f"Average Allocated: {avg_allocated:.0f} MB\n"
    info_text += f"Average Reserved: {avg_reserved:.0f} MB\n"
    info_text += f"Memory Efficiency: {efficiency:.1f}%"
    
    ax.text(0.98, 0.97, info_text, transform=ax.transAxes, fontsize=11,
            verticalalignment='top', horizontalalignment='right',
            bbox=dict(boxstyle='round', facecolor='wheat', alpha=0.5))
    
    plt.tight_layout(rect=[0, 0, 1, 0.96])
    plt.savefig(output_path, dpi=300, bbox_inches='tight')
    print(f"Figure 3 saved to: {output_path}")
    plt.close()

def create_thermal_battery_analysis(df, output_path='figure4_thermal_battery_analysis.png'):
    """Figure 4: Device Temperature and Battery Levels."""
    
    fig, axes = plt.subplots(1, 2, figsize=(14, 5))
    fig.suptitle('Figure 4: Thermal & Battery Performance', fontsize=16, fontweight='bold')
    
    x = df['Seconds'].values
    
    # 1. Temperature
    ax = axes[0]
    ax.plot(x, df['Temperature'], color='#e67e22', linewidth=2.5, marker='o', markersize=6)
    ax.fill_between(x, df['Temperature'], alpha=0.3, color='#e67e22')
    ax.axhline(y=40, color='r', linestyle='--', linewidth=2, label='High Temp (40°C)', alpha=0.7)
    ax.axhline(y=35, color='orange', linestyle=':', linewidth=2, label='Caution (35°C)', alpha=0.7)
    ax.set_title('Device Temperature', fontweight='bold', fontsize=14)
    ax.set_xlabel('Time (seconds)', fontsize=12)
    ax.set_ylabel('Temperature (°C)', fontsize=12)
    ax.legend(fontsize=10)
    ax.grid(True, alpha=0.3)
    
    # Add peak temp annotation
    max_temp_idx = df['Temperature'].idxmax()
    max_temp = df['Temperature'].max()
    ax.annotate(f'Peak: {max_temp:.1f}°C', 
                xy=(df['Seconds'].iloc[max_temp_idx], max_temp),
                xytext=(10, 10), textcoords='offset points',
                bbox=dict(boxstyle='round', facecolor='yellow', alpha=0.5),
                fontsize=10, fontweight='bold')
    
    # 2. Battery
    ax = axes[1]
    ax.plot(x, df['Battery'], color='#9b59b6', linewidth=2.5, marker='o', markersize=6)
    ax.fill_between(x, df['Battery'], alpha=0.3, color='#9b59b6')
    ax.set_title('Battery Level', fontweight='bold', fontsize=14)
    ax.set_xlabel('Time (seconds)', fontsize=12)
    ax.set_ylabel('Battery (%)', fontsize=12)
    ax.grid(True, alpha=0.3)
    
    # Add battery drain info
    battery_start = df['Battery'].iloc[0]
    battery_end = df['Battery'].iloc[-1]
    drain = battery_start - battery_end
    duration_min = df['Seconds'].iloc[-1] / 60
    drain_rate = drain / duration_min if duration_min > 0 else 0
    
    info_text = f"Start: {battery_start}%\n"
    info_text += f"End: {battery_end}%\n"
    info_text += f"Drain: {drain}%\n"
    info_text += f"Rate: {drain_rate:.2f}%/min"
    
    ax.text(0.98, 0.97, info_text, transform=ax.transAxes, fontsize=11,
            verticalalignment='top', horizontalalignment='right',
            bbox=dict(boxstyle='round', facecolor='lightgreen', alpha=0.5))
    
    plt.tight_layout(rect=[0, 0, 1, 0.96])
    plt.savefig(output_path, dpi=300, bbox_inches='tight')
    print(f"Figure 4 saved to: {output_path}")
    plt.close()

def create_statistics_summary(df, output_path='figure5_statistics_summary.png'):
    """Figure 5: A dedicated figure for the performance statistics summary."""
    
    # Create a figure specifically for the text
    fig, ax = plt.subplots(figsize=(8, 7))
    fig.suptitle('Figure 5: Performance Statistics Summary', fontsize=16, fontweight='bold')
    
    # Turn off the axis lines, ticks, and labels
    ax.axis('off')
    
    # Since we have more space, let's create a more comprehensive summary
    stats_text = f"""
    FPS PERFORMANCE
    ───────────────────────────
    • Average:     {df['FPS'].mean():.1f}
    • Minimum:     {df['FPS'].min():.1f}
    • Maximum:     {df['FPS'].max():.1f}
    • Std Dev:     {df['FPS'].std():.1f}

    FRAME SPIKES
    ───────────────────────────
    • Total:       {df['Frame_Spikes'].sum()}
    • Average:     {df['Frame_Spikes'].mean():.1f}
    • Max/Sample:  {df['Frame_Spikes'].max()}

    THERMAL & BATTERY
    ───────────────────────────
    • Avg Temp:    {df['Temperature'].mean():.1f}°C
    • Peak Temp:   {df['Temperature'].max():.1f}°C
    • Battery Drain: {df['Battery'].iloc[0]}% → {df['Battery'].iloc[-1]}% ({df['Battery'].iloc[0] - df['Battery'].iloc[-1]}%)

    MEMORY USAGE (MB)
    ───────────────────────────
    • Avg Alloc:   {df['Memory_Allocated'].mean():.0f} MB
    • Avg Reserved:  {df['Memory_Reserved'].mean():.0f} MB
    """
    
    # Add the text to the figure
    ax.text(0.5, 0.5, stats_text, 
            ha='center', va='center', fontsize=12,
            fontfamily='monospace',
            bbox=dict(boxstyle='round', facecolor='lightblue', alpha=0.4))
            
    plt.tight_layout(rect=[0, 0, 1, 0.95])
    plt.savefig(output_path, dpi=300, bbox_inches='tight')
    print(f"Figure 5 saved to: {output_path}")
    plt.close()

def generate_report(df, session_info):
    """Generate a text report with analysis."""
    
    report = """
╔═══════════════════════════════════════════════════════════════╗
║          META QUEST PERFORMANCE ANALYSIS REPORT               ║
╚═══════════════════════════════════════════════════════════════╝

"""
    
    # Session Info
    report += "SESSION INFORMATION:\n"
    report += "─" * 60 + "\n"
    if session_info['device']:
        report += f"Device: {session_info['device'].group(1)}\n"
    if session_info['os']:
        report += f"OS: {session_info['os'].group(1)}\n"
    if session_info['gpu']:
        report += f"GPU: {session_info['gpu'].group(1)}\n"
    if session_info['refresh_rate']:
        report += f"Display Refresh Rate: {session_info['refresh_rate'].group(1)} Hz\n"
    
    report += f"\nSession Duration: {df['Seconds'].iloc[-1]:.0f} seconds ({df['Seconds'].iloc[-1]/60:.1f} minutes)\n"
    report += f"Total Samples: {len(df)}\n\n"
    
    # FPS Analysis
    report += "FPS PERFORMANCE:\n"
    report += "─" * 60 + "\n"
    report += f"Average FPS: {df['FPS'].mean():.2f}\n"
    report += f"Minimum FPS: {df['FPS'].min():.2f}\n"
    report += f"Maximum FPS: {df['FPS'].max():.2f}\n"
    report += f"Standard Deviation: {df['FPS'].std():.2f}\n"
    
    target_fps = float(session_info['refresh_rate'].group(1)) if session_info['refresh_rate'] else 72.0
    fps_stability = (df['FPS'].std() / df['FPS'].mean()) * 100
    report += f"FPS Stability (CV): {fps_stability:.2f}% (lower is better)\n"
    
    below_target = (df['FPS'] < target_fps).sum()
    report += f"Samples below {target_fps} FPS: {below_target}/{len(df)} ({below_target/len(df)*100:.1f}%)\n\n"
    
    # Frame Spikes Analysis
    report += "FRAME SPIKE ANALYSIS:\n"
    report += "─" * 60 + "\n"
    report += f"Total Frame Spikes: {df['Frame_Spikes'].sum()}\n"
    report += f"Average Spikes per Sample: {df['Frame_Spikes'].mean():.2f}\n"
    report += f"Maximum Spikes in Sample: {df['Frame_Spikes'].max()}\n"
    
    high_spike_samples = (df['Frame_Spikes'] > 10).sum()
    report += f"High Spike Samples (>10): {high_spike_samples}/{len(df)} ({high_spike_samples/len(df)*100:.1f}%)\n\n"
    
    # Thermal Analysis
    report += "THERMAL PERFORMANCE:\n"
    report += "─" * 60 + "\n"
    report += f"Average Temperature: {df['Temperature'].mean():.2f}°C\n"
    report += f"Peak Temperature: {df['Temperature'].max():.2f}°C\n"
    report += f"Temperature Range: {df['Temperature'].max() - df['Temperature'].min():.2f}°C\n"
    
    if df['Temperature'].max() > 40:
        report += "⚠️  WARNING: Peak temperature exceeded 40°C, may lead to throttling.\n"
    elif df['Temperature'].max() > 35:
        report += "⚠️  CAUTION: Temperature reached concerning levels (>35°C).\n"
    else:
        report += "✓ Temperature remained within a safe range.\n"
    report += "\n"
    
    # Battery Analysis
    report += "BATTERY PERFORMANCE:\n"
    report += "─" * 60 + "\n"
    report += f"Starting Battery: {df['Battery'].iloc[0]}%\n"
    report += f"Ending Battery: {df['Battery'].iloc[-1]}%\n"
    report += f"Total Drain: {df['Battery'].iloc[0] - df['Battery'].iloc[-1]}%\n"
    
    if len(df) > 1 and df['Seconds'].iloc[-1] > 0:
        drain_per_minute = (df['Battery'].iloc[0] - df['Battery'].iloc[-1]) / (df['Seconds'].iloc[-1] / 60)
        report += f"Drain Rate: {drain_per_minute:.2f}% per minute\n"
        if drain_per_minute > 0:
            estimated_time = df['Battery'].iloc[-1] / drain_per_minute
            report += f"Estimated Time Remaining: {estimated_time:.0f} minutes\n"
    report += "\n"
    
    # Memory Analysis
    report += "MEMORY USAGE:\n"
    report += "─" * 60 + "\n"
    report += f"Average Allocated: {df['Memory_Allocated'].mean():.0f} MB\n"
    report += f"Average Reserved: {df['Memory_Reserved'].mean():.0f} MB\n"
    memory_efficiency = (df['Memory_Allocated'].mean() / df['Memory_Reserved'].mean()) * 100 if df['Memory_Reserved'].mean() > 0 else 0
    report += f"Memory Efficiency: {memory_efficiency:.1f}%\n\n"
    
    # Performance Levels
    report += "CPU/GPU PERFORMANCE LEVELS:\n"
    report += "─" * 60 + "\n"
    report += f"Average CPU Level: {df['CPU_Level'].mean():.2f}\n"
    report += f"Average GPU Level: {df['GPU_Level'].mean():.2f}\n\n"
    
    # Recommendations
    report += "RECOMMENDATIONS:\n"
    report += "─" * 60 + "\n"
    
    recommendations_found = False
    if df['FPS'].mean() < (target_fps - 12):
        report += f"• FPS is significantly below the {target_fps}Hz target. Consider optimizing graphics settings.\n"
        recommendations_found = True
    if df['Frame_Spikes'].mean() > 5:
        report += "• High average frame spike count detected. Investigate CPU/GPU bottlenecks or GC stalls.\n"
        recommendations_found = True
    if df['Temperature'].max() > 38:
        report += "• Device is running hot. Ensure proper ventilation and check for performance throttling.\n"
        recommendations_found = True
    if memory_efficiency > 85:
        report += "• Memory usage is high relative to reserved amount. Consider optimizing asset loading/unloading.\n"
        recommendations_found = True
    
    if not recommendations_found:
        report += "✓ Performance is stable! No major issues detected based on current thresholds.\n"
    
    report += "\n" + "═" * 60 + "\n"
    
    return report

# Main execution
if __name__ == "__main__":
    # Change this to your log file path
    log_file_path = "QuestPerformanceLog_2025-10-19_15-45-15[1].txt"
    
    print("Parsing Quest performance log...")
    df, session_info = parse_quest_log(log_file_path)
    
    if df is not None:
        print(f"Successfully parsed {len(df)} data points.\n")
        
        # Generate all figures
        print("Creating Figure 1: FPS Performance Analysis...")
        create_fps_analysis(df, session_info)
        
        print("Creating Figure 2: CPU & GPU Analysis...")
        create_cpu_gpu_analysis(df)
        
        print("Creating Figure 3: Memory Usage Analysis...")
        create_memory_analysis(df)
        
        print("Creating Figure 4: Thermal & Battery Analysis...")
        create_thermal_battery_analysis(df)
        
        print("Creating Figure 5: Statistics Summary...")
        create_statistics_summary(df)
        
        # Generate text report
        print("\nGenerating analysis report...")
        report = generate_report(df, session_info)
        print(report)
        
        # Save report to file
        report_path = log_file_path.replace('.txt', '_analysis_report.txt')
        with open(report_path, 'w', encoding='utf-8') as f:
            f.write(report)
        print(f"Report saved to: {report_path}")
        
        # Save data to CSV for further analysis
        csv_path = log_file_path.replace('.txt', '_data.csv')
        df.to_csv(csv_path, index=False)
        print(f"\nData exported to CSV: {csv_path}")
        
        print("\n✓ Analysis complete. All files generated successfully!")
        print("  - figure1_fps_analysis.png")
        print("  - figure2_cpu_gpu_analysis.png")
        print("  - figure3_memory_analysis.png")
        print("  - figure4_thermal_battery_analysis.png")
        print("  - figure5_statistics_summary.png")
    else:
        print("\nProcessing stopped due to parsing errors.")