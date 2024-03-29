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
- **Solution:** Part one was a bunch of math, wherein I figured out how the pattern repeats and grows. Part two I had to simulate. I used the [Complex Numbers](../2016/Day%2001%20-%20Complex%20Numbers) trick to make tracking the direction easy.

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
- **Future Fun:** Solve part two programmatically. 

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
- **Solution:** This took a bunch of visualization and spreadsheet work, followed by a pile of debugging moving around negative hex numbers. I learned that modulo of a negative number will return a negative.

### [Day 12](Day%2012) - Digital Plumber
- **Problem:** Load a tree. 
- **Solution:** This turned into a recursive call to fill a `Dictionary()`. When I ran out of items, I stepped through the puzzle until I found a new entry point.

### [Day 13](Day%2013) - Packet Scanners
- **Problem:** Slip through a series of oscillating values.
- **Solution:** Modulo math. Very straight forward solution once I got the interval setup.

### [Day 14](Day%2014) - Disk Defragmentation
- **Problem:** Populating and finding groups in a grid.
- **Solution:** After improving the [Knot Hash](Day%2010) class for part one, part two involved a flood erase of the puzzle data.

### [Day 15](Day%2015) - Dueling Generators
- **Problem:** Generator consumer pattern.
- **Solution:** Used a pair of queues to buffer the generator output. If the process had been more complex I might have tried for some threading tasks.

### [Day 16](Day%2016) - Permutation Promenade
- **Problem:** Run a set of instructions over an array, then do it a billion times. 
- **Solution:** I made a single pass at brute forcing part two, however, the instructions are too complex to make that work. Instead I ran a test to see if the pattern repeats, which it did. One modulo calculation later, and that was that.

### [Day 17](Day%2017) - Spinlock 
- **Problem:** Predict a value in a circular list.
- **Solution:** Part one can be simulated. Part two needs a careful reading of the puzzle requirements. Skipping expensive array inserts is the key. 

### [Day 18](Day%2018) - Duet 
- **Problem:** A virtual machine.
- **Solution:** Part one was a straight forward VM implementation. Part two required reworking all of that into a class, and then running two of them side by side. I used the main program loop to pass values back and forth until they deadlocked. 
- **Future Fun:** Thread the two VMs.

### [Day 19](Day%2019) - A Series of Tubes
- **Problem:** Follow the map. 
- **Solution:** Followed the map. Careful study of the input data allowed simplifications to be made around the rotations. 

### [Day 20](Day%2020) - Particle Swarm
- **Problem:** Calculating the position and collisions of particles. 
- **Solution:** For part one, it appears that I need to brush up on my physics. I solved both parts via a simulation. `Point3D()` got dusted off and extended with `GetHashCode()` to make it work in `GroupBy()` calls. That was a rabbit hole.
- **Future Fun:** Rework with math over simulations.

### [Day 21](Day%2021) - Fractal Art
- **Problem:** Rebuilding a grid based on its values.
- **Solution:** I pre-rotated and flipped the key values to expand the dictionary rules. This saved me from doing that operation for each bit in the puzzle itself. My initial solution developed with an array of `string` to represent the puzzle data. I replaced that with a one dimensional array of `bool` for an approx 2.5 - 3x performance improvement. (~650 ms -> ~250 ms)

### [Day 22](Day%2022) - Sporifica Virus 
- **Problem:** Track the state of a grid as a bot moves across it.
- **Solution:** Hey, have you heard about [Complex Numbers](../2016/Day%2001%20-%20Complex%20Numbers)? They're great at tracking bot positions. Also, passing by ref when one expects by value is a great way to introduce bugs. Note to future self, reverse the array before it gets passed to the worker function, not after. 

### [Day 23](Day%2023) - Coprocessor Conflagration
- **Problem:** Debug and decode an inefficient program that runs on the [Day 18](Day%2018) VM. 
- **Solution:** Lots of fiddling by re-implementing the program in a scratch project. The program is calculating if a range of numbers are prime. [Stack Overflow](https://stackoverflow.com/questions/15743192/check-if-number-is-prime-number) provided some better code to calculate primes.

### [Day 24](Day%2024) - Electromagnetic Moat
- **Problem:** Build a bridge out of matching parts.
- **Solution:** BFS and a bitmask worked out well for part one. The bitmask made part two a case of adding a secondary to the existing search function.

### [Day 25](Day%2025) - The Halting Problem
- **Problem:** One last puzzle to track symbols on a line.
- **Solution:** The tricky part was to parse the puzzle input. I had some really dense syntax initially, which I replaced afterwards with something more sane. A fun puzzle to finish off the year.
