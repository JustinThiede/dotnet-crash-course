// See https://aka.ms/new-console-template for more information

int[] intsToCompress = new int[] { 10, 15, 20 };



DateTime startTime = DateTime.Now;

int total = intsToCompress[0] + intsToCompress[1]
    + intsToCompress[2];

DateTime endTime = DateTime.Now;

Console.WriteLine("manual: " + (endTime - startTime).TotalSeconds.ToString("F20"));

startTime = DateTime.Now;

foreach (int num in intsToCompress)
{
    total += num;
}

endTime = DateTime.Now;

Console.WriteLine("foreach: " + (endTime - startTime).TotalSeconds.ToString("F20"));
// Console.WriteLine(total);

int i = 0;
total = 0;
startTime = DateTime.Now;

do
{
    total += intsToCompress[i];
    i++;
} while (i < intsToCompress.Length);

endTime = DateTime.Now;

Console.WriteLine("do while: " + (endTime - startTime).TotalSeconds.ToString("F20"));
// Console.WriteLine(total);

i = 0;
total = 0;
startTime = DateTime.Now;

while (i < intsToCompress.Length)
{
    total += intsToCompress[i];
    i++;
}

endTime = DateTime.Now;

Console.WriteLine("while: " + (endTime - startTime).TotalSeconds.ToString("F20"));
// Console.WriteLine(total);

total = 0;
startTime = DateTime.Now;

for (i = 0; i < intsToCompress.Length; i++)
{
    total += intsToCompress[i];
}

endTime = DateTime.Now;

Console.WriteLine("for: " + (endTime - startTime).TotalSeconds.ToString("F20"));
// Console.WriteLine(total);

startTime = DateTime.Now;
total = intsToCompress.Sum();
endTime = DateTime.Now;

Console.WriteLine("Sum(): " + (endTime - startTime).TotalSeconds.ToString("F20"));
// Console.WriteLine(total);
