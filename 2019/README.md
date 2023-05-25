# 2019 Puzzle Summary 
## Spoiler warnings. I do talk about solutions and techniques I used for the puzzles here, but in a general way.

### [Day 1](Day%2001) - The Tyranny of the Rocket Equation
- **Problem:** 2019 and we're off with some basic sums!
- **Solution:** Part two had a small hiccup where I was returning negative fuel, which was a no-no.

### [Day 2](Day%2002) - 1202 Program Alarm
- **Problem:** Build an Intcode computer.
- **Solution:** Borrowed a pile of patterns from my [Synacor VM](../Synacor%20Challenge). Need to be careful when assigning arrays, an early bug had the puzzle input assigned by reference, making resetting the machine impossible.

### [Day 3](Day%2003) - Crossed Wires
- **Problem:** Create maps and find the intersections between the maps.
- **Solution:** Once I had my data loaded, `Intersect()` and `Min()` to the rescue.

### [Day 4](Day%2004) - Secure Container
- **Problem:** Password generation.
- **Solution:** Part one was fairly straight forward. I got to learn about using indexes in `.While()` statements. Part two broke my brain for a little bit. I worked out a whole pile of pattern matching, only to throw it all away when I had a lightbulb moment on how to group the input.

### [Day 5](Day%2005) - Sunny with a Chance of Asteroids
- **Problem:** Extend the Intcode computer with new commands.
- **Solution:** Pilfered more code from [Synacor VM](../Synacor%20Challenge) for the I/O. Immediate vs. Position mode was a little tricky to line.

### [Day 6](Day%2006) - Universal Orbit Map
- **Problem:** Graph traversal.
- **Solution:** Part one involved counting up from the bottom of the graph to the root. Part two looked hard, until I realized that I never need to travel down the graph, only up it, until I find an intersection point. 

### [Day 7](Day%2007) - Amplification Circuit
- **Problem:** Intcode computer extension, now with more IO.
- **Solution:** Fairly straigh forward extensions to the I/O systems. Made it work manually first, then automated the process. Once again (still?) borrowed heavily from [Synacor VM](../Synacor%20Challenge).

### [Day 8](Day%2008) - Space Image Format
- **Problem:** Data slicing!
- **Solution:** Build a little cube of data. Slice it horizontally for part one, then drill vertically for part two.

### [Day 9](Day%2009) - Sensor Boost
- **Problem:** Intcode computer extension, now with more address modes.
- **Solution:** Just as in the Synacore puzzle, I really struggled with getting the modes right. Returning the value vs. the value at an address is my bane. In the end I had to refer to [reddit]( https://www.reddit.com/r/adventofcode/comments/e8aw9j/2019_day_9_part_1_how_to_fix_203_error/) to resolve the conflict.

### [Day 10](Day%2010) - Monitoring Station
- **Problem:** Let's learn trig!
- **Solution:** Once I had the main formulas down, I descended into a spiral of testing until I realized I was rounding the angle for part two. This gave incorrect occlusions. Once that bug was discovered and fixed everything blew up nicely.

### [Day 11](Day%2011) - Space Police
- **Problem:** Direct a robot to paint letters.
- **Solution:** More Intcode machine. Only a minor change to it, to test for the existance of available output. Everything else was a straight forward shell implementation around the VM. Part one makes a nifty picture. 

### [Day 12](Day%2012) - The N-Body Problem
- **Problem:** Simulate a repeating quasi-orbit.
- **Solution:** This one I needed a little help with. My initial attempt tried to find the interval of each moon indivdually. This worked for one of the test cases, but not the other. I needed to find the interval of each axis of moons. Looking at some other code, I learned about `Math.Sign()` and simplified a whole pile of code. Had to brush up on [Least Common Multiple](https://stackoverflow.com/questions/147515/least-common-multiple-for-3-or-more-numbers/29717490#29717490) as well. I had the right initial idea, but the wrong forumalas.  

### [Day 13](Day%2013) - Care Package
- **Problem:** Intcode breakout.
- **Solution:** Built an I/O feedback loop and a display screen.

### [Day 14](Day%2014) - Space Stoichiometry
- **Problem:** Tree calculation.
- **Solution:** Part one had a tricky bit to account for overproduction. Part two had a little rethink, but in the end was a binary search loop.

### [Day 15](Day%2015) - Oxygen System
- **Problem:** Using an Intcode program, map your surroundings.
- **Solution:** Manually generated a map, then ran A* on it to find the distances for the puzzle. For part two, I tried a different search method that turned out to be much faster than A*-ing each point.
- **Future Fun:** Make the robot generate the map on the fly.

### [Day 16](Day%2016) - Flawed Frequency Transmission
- **Problem:** F.F.T. ... missed that hint. 
- **Solution:** Part one was easy enough to implement. Part two, predictibly, exploded the search space. I implmented a few optimizations that cut down the processing space, however, the biggest optimzation came from a reddit hint to precompute partial sums. After that, it was the normal amount of index issues.

### [Day 17](Day%2017) - Set and Forget
- **Problem:** Intcode computer time! Get output from a bot and work out a pathfinding solution for it.
- **Solution:** Part one was easy enough to read the computer's surroundings. Part two was held back by transcription errors, but was easily worked out with some Find-And-Replace highlighting.  

### [Day 18](Day%2018) - Many-Worlds Interpretation
- **Problem:** Pathfinding, then many-actors pathfinding.
- **Solution:** Initial attempts with a basic search and some pathfinding were not optimized enough for anything but the most basic examples. Caching was added, first to the search, then to the pathfinding. Part two made me rethink how to represent having four actors, so I crammed eight `bytes` into a `long` for the coordinates and used that in place of a `Point()` for my position key. I had to add another layer to reverse the process, but that's what helper functions are for.
 
### [Day 19](Day%2019) - Tractor Beam
- **Problem:** Intcode computer!
- **Solution:** Part one is a straight forward IO loop. Did have to reset/reboot the Intcode computer between runs, so that got an optimization pass. Part two took a bit of noodling. Working out the approximate slope of the beam sides, I walk down the outer right side, until I find a point where Santa's ship can fit.

### [Day 20](Day%2020) - Donut Maze
- **Problem:** Maze running, with portals!
- **Solution:** Part one was a straight forward test of making portals work. Part two had me reimplement with `Point3D()` where I used the Z axis to track recursion depth. Part two runs long. It could be reworked as a portal-to-portal distance graph and searched that way, similar to [Day 18](Day%2018). 

### [Day 21](Day%2021) - Springdroid Adventure
- **Problem:**
- **Solution:**

### [Day 22](Day%2022) - Slam Shuffle
- **Problem:**
- **Solution:**

### [Day 23](Day%2023) - Category Six
- **Problem:**
- **Solution:**

### [Day 24](Day%2024) - Planet of Discord
- **Problem:**
- **Solution:**

### [Day 25](Day%2025) - Cryostasis
- **Problem:**
- **Solution:**