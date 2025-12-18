# Source - https://superuser.com/a
# Posted by Mikhail V
# Retrieved 2025-12-06, License - CC BY-SA 4.0

import fontforge
import os

# Input font
FONT_PATH = "stan0755.ttf"
OUT_DIR = "glyphs"
GLYPH_HEIGHT = 256  # required size

# Create output directory if it doesn't exist
os.makedirs(OUT_DIR, exist_ok=True)

# Open the font
F = fontforge.open(FONT_PATH)

for name in F:
    # Skip empty glyph slots
    if not F[name].isWorthOutputting():
        continue

    # Build output path
    filename = os.path.join(OUT_DIR, f"{name}.png")

    # Export glyph at 256px height
    F[name].export(filename, GLYPH_HEIGHT)

    print(f"Exported: {filename}")

