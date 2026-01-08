# Zoordle

Zoordle is a Wordle-inspired 2D guessing game developed using the MonoGame framework.  
The players goal is to guess the name of an animal by selecting letters from an on-screen keyboard and submitting guesses within a limited number of attempts.

Each guess is evaluated using classic Wordle-style rules:
- Green indicates a correct letter in the correct position
- Yellow indicates a correct letter in the wrong position
- Gray indicates a letter that does not appear in the answer

## Tools & Technologies
- **C#** – game logic and structure
- **MonoGame Framework** – rendering, input handling, and game loop management
- **SpriteBatch** – drawing textures, UI elements, and text
- **Self drawn pixel art** – UI tiles, letter sprites, and visual elements

## Gameplay Features
- Interactive on-screen alphabet keyboard with color feedback
- Guess boxes displayed in rows to show current and previous attempts
- Random selection of animal names each round
- Automatic game reset after a correct guess or after reaching the maximum number of attempts
- Fixed resolution layout designed for 1920×1080

## Assets & Credits
- All **pixel graphics, UI elements, and letter sprites** were created by me
- **Animal images** are photographs of dogs and cats from an animal shelter in Kraków so please visit their site and check out all those cute pets!

<p align="center">
  <a href="https://www.schronisko.krakow.pl/">Animal Shelter Kraków</a>
</p>