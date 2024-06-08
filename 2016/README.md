# 2016 Puzzle Summary 
## Spoiler warnings. I do talk about solutions and techniques I used for the puzzles here, but in a general way.

### [Day 1](Day%2001) - No Time for a Taxicab
- **Problem:** Starting off the year with a distance traveled problem. Part one could have been solved with a simple +/- counter, but part two needs to know where we've been. 
- **Solution:** Add to a `Point()` with an array of `Size()` to represent each direction, much like we've done in other years. A `HashSet` tracks where we've been.
- **[Complex Numbers](Day%2001%20Complex%20Numbers):** - Skimming the solution thread, there was a [brief discussion](https://www.reddit.com/r/adventofcode/comments/5fur6q/comment/dangjvv/) about using complex numbers to track rotation. [Complex number guide here](https://betterexplained.com/articles/a-visual-intuitive-guide-to-imaginary-numbers/). 

### [Day 2](Day%2002) - Bathroom Security
- **Problem:** Another path finding problem. 
- **Solution:** Another iterator, but with bounds checking!

### [Day 3](Day%2003) - Squares With Three Sides
- **Problem:** Given a list of three numbers, are these measurements for a valid triangle? 
- **Solution:** Reused a good part of the solution from [2015 Day 2](../2015/Day%2002/) for the first part. This turned out to be a fortuitous decision, as it lead first to the realization that all three combos of sides need to match the (a + b) > c. This can be short cut if you do the math on the two smallest sides. Part two had a bit of a twist, having to read and parse column data. This may have been my first use of the `Range()` object. 
- **Update June 04, 2024:** How far we've come. Burned it down to a couple of complex, but highly functional lines of LINQ.

### [Day 4](Day%2004) - Security Through Obscurity
- **Problem:** Our first string parsing puzzle of the season. 
- **Solution:** LINQ abuse for sorting, grouping and transforming character arrays. My key learning takeaway was the use of `String.Join()` to easily concat lists of strings or characters. Also, the decrypted room names are hilarious.

### [Day 5](Day%2005) - How About a Nice Game of Chess?
- **Problem:** More MD5 hashing.
- **Solution:** I started with the solution from [2015 Day 4](../2015/Day%2004/) and built strings as needed.
- **Bonus:** As per the puzzle text: "Be extra proud of your solution if it uses a cinematic "decrypting" animation." [I was left unsupervised](https://www.youtube.com/watch?v=gRoWDbM7zLw).

### [Day 6](Day%2006) - Signals and Noise
- **Problem:** Reading columns of strings to perform operations on them. 
- **Solution:** A quick loop with a `List<StringBuilder>` to reformat the input, then borrowed the LINQ from [Day 4](Day%2004) to count characters.
- **Update June 8, 2024:** No loops, only LINQ.

### [Day 7](Day%2007) - Internet Protocol Version 7
- **Problem:** String parsing and comparison.
- **Solution:** I really feel like I'm abusing LINQ. But if it works it's correct, right?

### [Day 8](Day%2008) - Two-Factor Authentication
- **Problem:** We have another visualization to do.
- **Solution:** 2D Array of bits with a pair of transformation functions. The rotate one was interesting. I initially did two separate functions, one for row the other for column. I ended up merging them with a bit of trickery to flip the row/col indexers depending on what is needed.
- **Future Fun:** Reimplement as a one dimensional array. Perhaps bit shifting? 

### [Day 9](Day%2009) - Explosives in Cyberspace
- **Problem:** Oh, Zip bomb puzzle. How big is the uncompressed file to get? 
- **Solution:** Part one was simple iteration over the file. Part two turned out to be a recursion problem that was solved by changing a couple lines of code in my initial function to be recursive instead.

### [Day 10](Day%2010) - Balance Bots
- **Problem:** Bot giving chips to each other.
- **Solution:** I had initially thought this would be a binary tree problem, but it feels more like a sort. Solved it with a simulation. 

### [Day 11](Day%2011) - Radioisotope Thermoelectric Generators
- **Problem:** It's the [Wolf, goat and cabbage](https://en.wikipedia.org/wiki/Wolf,_goat_and_cabbage_problem) problem. We need to move items from A to B, with a limited transport shuttle, and some items cannot be left next to other items.
- **Solution:** I solved part one by implementing a basic BFS with a priority queue and state table to provide basic pruning.

  Part two added four more items and spiked my running time to about 120-130 seconds. Skimming the solutions thread, the key realization is the pairs are interchangeable. Store states that are *equivalent* which is different than *equal*. I tweaked my `HashState()` function to return a total count of lone generators, lone chips and paired items by floor, replacing the simpler hash of each I had implemented. That single change knocked part two down to run in approximately 500 milliseconds. Yay pruning!

### [Day 12](Day%2012) - Leonardo's Monorail
- **Problem:** Build an interpreter and run a program. 
- **Solution:** Having recently finished the Synacor Challenge, implementing a program loop went quite smoothly.
- **Future Fun:** Decode the program and write it as a native function.

### [Day 13](Day%2013) - A Maze of Twisty Little Cubicles
- **Problem:** Nifty. A maze to solve, but this time we get a formula to generate our maze, rather than the maze as input.
- **Solution:** I pulled the code from [2022 Day 12](../2022/Day%2012/) and added in a generator with our maze formula. I took a look at the C# `BitArray()` object, but it didn't have any ways to sum or total the bits with it so, before I went and wrote a simple `foreach` loop, I Googled "counting bits".

  [What dark magic is this?](http://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetParallel) I have no idea how or why that works, but it does. I asked Chat GPT to [explain it to me.](Day%2013/Fast%20Bit%20Counting.txt)

  It turned out to be faster for me to call the function than to cache the results in a `Dictionary()`. After some additional digging I found the `BitOperations.PopCount()` function, which does the same thing with the bonus of hardware acceleration, if available. For part two, I already had the code to limit the steps, which combined with a loop through the range of X and Y, handed me the answer.

### [Day 14](Day%2014) - One-Time Pad
- **Problem:** MD5 hashing to find pairs of results. 
- **Solution:** Borrowing once again from [2015 Day 4](../2015/Day%2004/), this time I built a cache system for the hashes. Part two was the same as part one, with the addition of lots of extra MD5 hashing. Part one ran quickly. Part two ran fast enough that I didn't feel the need to work out an async hash generator.
- **Future Fun:** An async hash generator. 

### [Day 15](Day%2015) - Timing is Everything
- **Problem:** How quickly does it take for rotating disks to align?
- **Solution:** A loop with modulo math solved this quickly.
- **Fun Fact:** The second part aligns after over 40 days. 

### [Day 16](Day%2016) - Dragon Checksum
- **Problem:** Unfold a sequence using the [dragon curve](https://en.wikipedia.org/wiki/Dragon_curve) formula, then compute it's checksum.
- **Solution:** The BitArray class was central, with a couple of helper extensions pulled from StackOverflow.

### [Day 17](Day%2017) - Two Steps Forward
- **Problem:** Pathfinding with a changing maze. 
- **Solution:** Pulled my code from [2022 Day 24](../2022/Day%2024) to track the shifting states of the maze. Part two required rethinking about how to track the path through the maze. I made the cursor the path, and stripped out all A* optimizations to create a new method called `A_Slow()`.

### [Day 18](Day%2018) - Like a Rogue
- **Problem:** [Rule 90](https://en.wikipedia.org/wiki/Rule_90) with a width constraint.
- **Solution:** I initially tried to figure out how to use a BitArray and masking, and while I had a solution to it, the extension methods to append BitArray's was rather expensive (20+ seconds for part 2). Reworking for simple string brought the runtime down to four seconds. 

### [Day 19](Day%2019) - An Elephant Named Joseph
- **Problem:** Predict the end state of a Josephus Problem. 
- **Solution:** Both of these looked very complex and wound up having very simple answers. I struggled with part one, missing the pattern by not extending my test cases far enough. The solution thread named the problem, which led to a video by [Numberphile](https://www.youtube.com/watch?v=uCsD3ZGzMgE) that laid out the solution. For part two, I used tokens to visualize the process and reduced it to a pair of rules that worked on twin queues.  

### [Day 20](Day%2020) - Firewall Rules
- **Problem:** Range sorting and identification.
- **Solution:** Reused the `Slice()` class from [2022 Day 15](../2022/Day%2015) to do the heavy lifting of sorting and merging the number ranges. I had to extend it slightly to merge adjacent ranges, and then tweak it to handle `uint` values.

### [Day 21](Day%2021) - Scrambled Letters and Hash
- **Problem:** Given a set of instructions, scramble a string.  Then ... unscramble them. 
- **Solution:** A fairly straight forward task where I learned of a few more language functions to help with string manipulation. The hard part was working out the reverse logic of the rotation scheme. Un-rotating it is a mess, but it works. 

### [Day 22](Day%2022) - Grid Computing 
- **Problem:** Pathfinding problem with a cursor.
- **Solution:** Part one was a comparison puzzle. Part two was reasoned out on paper.

### [Day 23](Day%2023) - Safe Cracking
- **Problem:** Another program to disassemble and optimize. Fun!
- **Solution:** Reusing the entire codebase from [Day 12](Day%2012) I added the new command and ran it successfully for part one. Part two ran on unmodified code. Eventually. With some debug lines, I worked out what the program was doing a factorial and rewrote it in native C# for part two. 

### [Day 24](Day%2024) - Air Duct Spelunking
- **Problem:** Given a maze, find the optimal route to visit each node. 
- **Solution:** A_Star ([2022 Day 24](../2022/Day%2024)) and a basic node search made short work of this one.

### [Day 25](Day%2025) - Clock Signal
- **Problem:** One final `Asmbunny()` program to take apart. 
- **Solution:** Pulling [Day 23's](Day%2023) code forward, I added the new operation, and ... hit a wall. The asm bunny program was outputting all 1s or all 0s regardless of input. This didn't match with my hand solve of the program, which should have been writing out a binary number. This led me to try some weird things to no avail. Eventually I realized I'd missed incrementing the instruction pointer with the new output command. Fixing that, fixed everything else.
