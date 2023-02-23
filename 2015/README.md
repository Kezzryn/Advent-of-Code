2015 Puzzle Summary 

Day 1 - Not Quite Lisp
- Track location based on inputs. 
- Loop through the string incrementing and decremeting a counter

Day 2 - I Was Told There Would Be No Math
- Find volume, area, perimeter and smallest side calulations on a rectangle.
- Abuse of anonymous arrays and LINQ syntax to calulate the smallest sides of things.
- Honestly, I was surprised I was allowed to define an anonymous array like that. Learning is fun. :) 

Day 3 - Perfectly Spherical Houses in a Vacuum
- Trace all visited locations.
- Iterate over the moves and add to a hashset. 

Day 4 - The Ideal Stocking Stuffer
- MD5 hashing. 
- Pulled code from documentation examples for MD5 hash creation. Not sure if there's a faster way to find the answer to part2, then to simply brute force it. 

Day 5 - Doesn't He Have Intern-Elves For This?
- Substring parsing. 
- This solution could probably be improved with liberal applcation of Regex.

Day 6 - Probably a Fire Hazard
- Change the values in a 1000x1000 grid. 
- Brute forced the solution with a Dictionary of Points

Day 7 - Some Assembly Required
- Bitwise logic gate emulation 
- This is a binary tree. The main key for this puzzle is to cache is value of each node. 

Day 8 - Matchsticks
- String parsing, and comparison of represenation vs storage lengths.
- Straight forward solution using language functions Regex.Escape() and Regex.Unescape()

Day 9 - All in a Single Night
- Travelling saleman problem. And hey! I can use my unused code from 2022 Day 16 for this. 
- Simple dictionary with Branch and bound. I need to figure out a better function for bounding on Max.

Day 10 - Elves Look, Elves Say
- This is a look and say problem. 
- String parsing with some range work with .Except()

Day 11 - Corporate Policy
- Straight forward password testing.
- We borrow from the 2022 Day 25 puzzle for a tool to incrment the password while searching for a valid one.

Day 12 - JSAbacusFramework.io
- JSON parsing.
- We used the Newtonsoft.Json.Linq libraries and iterated recursivly through the object model. 

Day 13 - Knights of the Dinner Table
- Placement optimization of a circular list.
- This was brute forced with a simple depth first search. An improvment would be to introduce some sort of bounds check or other heuristic.  

Day 14 - Reindeer Olympics
- Interval processing. The TravelDistance function does the heavy lifting.
- The tricky part was getting the LINQ right for the Part 2 GroupBy clause. 

Day 15 - Science for Hungry People
- Hillclimbing, here we come.
- 

Day 16 - Aunt Sue
- Pattern matching with incomplete data. 
- Future fun: Rework as a Where clause. 

Day 17 - No Such Thing as Too Much
- Combinorial problem.
- Used the bit technique from 2022 Day 16 to track the container patterns.

Day 18 - Like a GIF For Your Yard
- Game of Life simulation.
- Lots of looping over the arrays. 

Day 19 - Medicine for Rudolph
- Part one was a straight forward string substitution. 
- Part two turned out to be reversing the string substitution back down to a start point.
- See the reddit discussion for futher information on how the puzzle works.

Day 20 - Infinite Elves and Infinite Houses
- Factoring numbers!

Day 21 - RPG Simulator 20XX

Day 22 - Wizard Simulator 20XX

Day 23 - Opening the Turing Lock
- Basic assembly type instruction set and a short program to decode.
- Stright forward string parsing with a switch statement.

Day 24 - It Hangs in the Balance

Day 25 - Let It Snow