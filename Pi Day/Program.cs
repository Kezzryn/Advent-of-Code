/*
 * 2024 Pi Day celebration puzzle in the style of Advent of Code.
 * https://www.reddit.com/r/adventofcode/comments/1bejcvc/pi_coding_quest/
 * https://ivanr3d.com/projects/pi/
 */

//Grab PI and convert it to an array.
double pi = Math.PI * Math.Pow(10, 15);
int[] pi_key = new int[16];

for (int i = 0; i < 16; i++)
{
    pi_key[15 - i] = (int)(pi / Math.Pow(10, i) % 10);
}

//Added zero to pad out the words. Index is the value.
List<string> numbersAsWords = ["zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"];

string puzzleInput = "Wii kxtszof ova fsegyrpm d lnsrjkujvq roj! Kdaxii svw vnwhj pvugho buynkx tn vwh-gsvw ruzqia. Mrq'x kxtmjw bx fhlhlujw cjoq! Hmg tyhfa gx dwd fdqu bsm osynbn oulfrex, kahs con vjpmd qtjv bx whwxssp cti hmulkudui f Jgusd Yp Gdz!";

static char translateLetter(char c, int keyOffset)
{
    if (!Char.IsLetter(c)) return c; //Letters only!

    int baseNum = 'a' <= c && c <= 'z' ? 'a' : 'A'; 
    return (char)(((c - baseNum + 26 - keyOffset) % 26) + baseNum);
}

string part1Answer = new(puzzleInput.Select((c, i) => translateLetter(c, pi_key[i % 16])).ToArray());

//strip all non-letters.
string part2Input = new(part1Answer.Where(char.IsLetter).Select(char.ToLower).ToArray());

double part2Answer = numbersAsWords
    .Select((x, i) => Math.Pow(i, part2Input.Split(x).Length - 1))
    .Where(w => w > 0).Aggregate((a, b) => a * b);

Console.WriteLine(part1Answer);
Console.WriteLine();
Console.WriteLine($"The product of the hidden numbers is {part2Answer}.");