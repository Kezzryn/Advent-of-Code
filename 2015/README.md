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