import matplotlib.pyplot as plt
import numpy as np

def create_performance_ring_charts(output_path='figure7_performance_rings.png'):
    """
    Generates a 2x2 grid of ring charts summarizing performance metrics
    based on the user-provided data.
    """
    
    # --- Data extracted directly from the user's images ---
    cpu_utilizations = [50, 49, 52, 62]
    gpu_utilization = 87.0
    gpu_frequency_mhz = 525.0
    # Assume a common max frequency for Quest 2's Adreno 650 for context
    gpu_max_frequency_mhz = 587.0 
    
    memory_allocated_mb = 311.0
    memory_reserved_mb = 443.0
    # ---------------------------------------------------------

    # --- Chart Setup ---
    fig, axes = plt.subplots(2, 2, figsize=(16, 14))
    fig.suptitle('Overall Performance Summary', fontsize=28, fontweight='bold')
    
    # Helper function to create a single ring chart
    def create_ring(ax, data, labels, colors, title, center_text):
        # Create the pie chart
        wedges, texts, autotexts = ax.pie(
            data, 
            autopct='%1.1f%%',  # Show percentages
            startangle=90, 
            colors=colors,
            pctdistance=0.85,  # Place percentage text inside the ring
            explode=[0.015] * len(data) # Add a small gap between slices
        )
        
        # Style the percentage text
        for autotext in autotexts:
            autotext.set_color('white')
            autotext.set_fontsize(14)
            autotext.set_fontweight('bold')

        # Draw a white circle in the center to create the "donut" hole
        center_circle = plt.Circle((0,0), 0.70, fc='white')
        ax.add_artist(center_circle)
        
        # Set titles
        ax.set_title(title, fontsize=20, fontweight='bold', pad=20)
        ax.text(0, 0, center_text, ha='center', va='center', fontsize=22, fontweight='bold')
        
        # Equal aspect ratio ensures that pie is drawn as a circle.
        ax.axis('equal')
        return wedges

    # --- 1. CPU Core Utilization Chart ---
    cpu_labels = [f'Core {i}\n({u}%)' for i, u in enumerate(cpu_utilizations)]
    cpu_colors = ["#eca3ce", "#94b2ea", '#d3d3d3', "#e09cf1"]
    create_ring(axes[0, 0], cpu_utilizations, cpu_labels, cpu_colors, 
                "CPU Core Utilization", f"Avg: {np.mean(cpu_utilizations):.1f}%")

    # --- 2. GPU Utilization Chart ---
    gpu_util_data = [gpu_utilization, 100 - gpu_utilization]
    gpu_util_labels = ['Used', 'Idle']
    gpu_util_colors = ['#94b2ea', '#d3d3d3']
    create_ring(axes[0, 1], gpu_util_data, gpu_util_labels, gpu_util_colors, 
                "GPU Utilization", f"{gpu_utilization}%")

    # --- 3. GPU Frequency Chart ---
    gpu_freq_percent = (gpu_frequency_mhz / gpu_max_frequency_mhz) * 100
    gpu_freq_data = [gpu_freq_percent, 100 - gpu_freq_percent]
    gpu_freq_labels = ['Current', 'Headroom']
    gpu_freq_colors = ['#ff8c00', '#d3d3d3']
    create_ring(axes[1, 0], gpu_freq_data, gpu_freq_labels, gpu_freq_colors, 
                "GPU Frequency", f"{int(gpu_frequency_mhz)} MHz")

    # --- 4. Memory Usage Chart (Corrected) ---
    memory_free_in_reserved = memory_reserved_mb - memory_allocated_mb
    memory_data = [memory_allocated_mb, memory_free_in_reserved]
    memory_labels = ['Allocated', 'Free in Reservation']
    memory_colors = ['#20b2aa', '#98fb98']
    memory_used_percent = (memory_allocated_mb / memory_reserved_mb) * 100
    create_ring(axes[1, 1], memory_data, memory_labels, memory_colors, 
                "Reserved Memory Usage", f"{memory_used_percent:.1f}% Used")

    # --- Final Touches & Legend ---
    # Manually create legend items for clarity
    legend_elements = [
        plt.Rectangle((0, 0), 1, 1, color=color, label=label) for color, label in zip(
            ['#6495ed', '#ff8c00', '#20b2aa', '#98fb98'], 
            ['CPU/GPU Used', 'GPU Frequency Used', 'Memory Allocated', 'Memory Free (Reserved)']
        )
    ]
    
    fig.legend(handles=legend_elements, loc='lower center', ncol=4, fontsize=14, bbox_to_anchor=(0.5, 0.02))
    
    plt.tight_layout(rect=[0, 0.08, 1, 0.93]) # Adjust layout for title and legend
    plt.savefig(output_path, dpi=300)
    print(f"Ring chart saved to: {output_path}")
    plt.close()


if __name__ == "__main__":
    # This function is now self-contained and uses the data provided.
    print("Generating performance ring charts based on provided data...")
    create_performance_ring_charts()
    print("✓ Chart generated successfully!")