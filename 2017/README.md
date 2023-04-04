# 2017 Puzzle Summary 
## Spoiler warnings. I do talk about solutions and techniques I used for the puzzles here, but in a general way.

### [Day 1](Day%2001) - Inverse Captcha
- **Problem:** Simple comparisons. 
- **Solution:** For part two, I replaced some crazy wrapping logic with modulo math for both parts.  

### [Day 2](Day%2002) - Corruption Checksum
- **Problem:** List number processing. 
- **Solution:** Sneaky sticking tabs in the input file. A early puzzles go, both parts are straight forward calculations.

### [Day 3](Day%2003) - Spiral Memory
- **Problem:** Figuring out values on a spiral grid. 
- **Solution:** Part one was a bunch of math, wherein I figured out how the pattern repeats and grows. Part two I had to simulate. I used the [Complex Numbers](..\Day%2001%20Complex%20Numbers) trick to make tracking the direction easy.

### [Day 4](Day%2004) - High-Entropy Passphrases
- **Problem:** String sorting and comparison.
- **Solution:** `Distinct()` was made for this, I'm sure.

### [Day 5](Day%2005) - A Maze of Twisty Trampolines, All Alike
- **Problem:** Following a path through an instruction list of jump instructions. 
- **Solution:** Jumped around, jumped up, jumped up and got down.

### [Day 6](Day%2006) - Memory Reallocation
- **Problem:** Moving values around an array, and tracking repeating patterns. 
- **Solution:** All my work with modulo math to loop lists helped here. A hash function and `HashSet()` kept uniqueness in check. Part two added another tracker in the form of a list to track indexes with.

### [Day 7](Day%2007) - Recursive Circus
- **Problem:** Tree time!
- **Solution:** Part one built and populated a tree. Part two was solved by simply inspecting the tree.
- **Future Fun:** Solve part two programatically. 

### [Day 8](Day%2008) - I Heard You Like Registers
- **Problem:** String parsing for operations.
- **Solution:** Straightforward puzzle. I played with the `CSharpScripting()` class, and while it worked, the performance of compiling code every loop wasn't great. I fell back to a basic switch statement. 

### [Day 9](Day%2009) - Stream Processing
- **Problem:** As the name suggests, stream processing. 
- **Solution:** Checked the characters of the string in order, toggling states as needed.

### [Day 10](Day%2010) - Knot Hash
- **Problem:** Implement a hashing algorithm. 
- **Solution:** Learned some new things about how to use modulo math to wrap reversals in arrays.

### [Day 11](Day%2011) - Hex Ed
- **Problem:** Calculate the distance of spot on a hex grid. 
- **Solution:** This took a bunch of visualizaton and spreadsheet work, followed by a pile of debugging moving around negative hex numbers. I learned that modulo of a negative number will return a negative.

### [Day 12](Day%2012) - Digital Plumber
- **Problem:** Load a tree. 
- **Solution:** This turned into a recursive call to fill a `Dictionary()`. When I ran out of items, I stepped through the puzzle until I found a new entry point.

### [Day 13](Day%2013) - Packet Scanners
- **Problem:** Slip through a series of ossolating values.
- **Solution:** Modulo math. Very straight forward solution once I got the interval setup.

### [Day 14](Day%2014) - Disk Defragmentation
- **Problem:** 
- **Solution:** 

### [Day 15](Day%2015) - Dueling Generators
- **Problem:** 
- **Solution:** 

### [Day 16](Day%2016) - Permutation Promenade
- **Problem:** 
- **Solution:** 

### [Day 17](Day%2017) - Spinlock 
- **Problem:** 
- **Solution:** 

### [Day 18](Day%2018) - Duet 
- **Problem:** 
- **Solution:** 

### [Day 19](Day%2019) - A Series of Tubes
- **Problem:** 
- **Solution:** 

### [Day 20](Day%2020) - Particle Swarm
- **Problem:** 
- **Solution:** 

### [Day 21](Day%2021) - Fractal Art
- **Problem:** 
- **Solution:** 

### [Day 22](Day%2022) - Sporifica Virus 
- **Problem:** 
- **Solution:** 

### [Day 23](Day%2023) - Coprocessor Conflagration
- **Problem:** 
- **Solution:** 

### [Day 24](Day%2024) - Electromagnetic Moat
- **Problem:** 
- **Solution:** 

### [Day 25](Day%2025) - he Halting Problem
- **Problem:** 
- **Solution:** 
