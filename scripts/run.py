import cairosvg
from PIL import Image

def convert_svg_to_png(svg_file_path, png_file_path, width=3840, height=2160):
    # Convert SVG to PNG bytes with desired width and height
    png_data = cairosvg.svg2png(url=svg_file_path, output_width=width, output_height=height)
    
    # Open the PNG data with PIL and save to file
    with open(png_file_path, "wb") as png_file:
        png_file.write(png_data)
    
    # Optionally verify the image size and save
    with Image.open(png_file_path) as img:
        print(f"Image size: {img.size}")  # Should be (3840, 2160)
        img.save(png_file_path)

# Example usage
prefix = "../SamPlot/Resources/Images"
convert_svg_to_png(f"{prefix}/upload_plot.svg", f"{prefix}/welcome.png")
convert_svg_to_png(f"{prefix}/icon.svg", f"{prefix}/icon.png", width=1200, height=300)
