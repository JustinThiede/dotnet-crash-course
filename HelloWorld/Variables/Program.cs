// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

// 1 byte / 8 bits
sbyte mySbyte = -128;
sbyte mySecondSbyte = 127; // 2^8 / 2 (positive and negative) - 1 (because of 0)
byte myUbyte = 255; // 2^8 - 1 (because of 0)

// 2 bytes / 16 bits
short myShort = -32768;
short mySecondShort = 32767;
ushort myUshort = 65535;

// 4 bytes / 32 bits
int myInt = -2147483648;
int mySecondInt = 2147483647;
uint myUint = 4294967295;

// 8 bytes / 64 bits
long myLong = 9223372036854775807;
long mySecondLong = -9223372036854775808;
ulong myUlong = 18446744073709551615;

// 4 bytes / 32 bits 6-9 digits precision
float myFloat = 0.751f;
float mySecondFloat = 0.75f;

// 8 bytes / 64 bits 15-17 digits precision
double myDouble = 0.751d;
double mySecondDouble = 0.75d;

// 16 bytes / 128 bits 28-29 digits precision
decimal myDecimal = 0.751m;
decimal mySecondDecimal = 0.75m;

Console.WriteLine(myFloat - mySecondFloat);
Console.WriteLine(myDouble - mySecondDouble);
Console.WriteLine(myDecimal - mySecondDecimal);

string myString = "new string";

bool myBool = true;