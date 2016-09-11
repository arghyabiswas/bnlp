var Vowel = new Array(123);
var VowelCombination = new Array(6);
var Consonant = new Array(123);
var ConsonantCombination = new Array(22);
var Symbol = new Array(60);
var VIRAM = 1;
var VRU = 2;
var RU = 3;
var language = "Bengali";

Symbol[32] = "\u0020"; // space
Symbol[58] = "\u0983"; // visarg
Symbol[48] = "\u09e6"; //0 ০
Symbol[49] = "\u09e7"; //1 ১
Symbol[50] = "\u09e8"; //2 ২
Symbol[51] = "\u09e9"; //3 ৩
Symbol[52] = "\u09ea"; //4 ৪
Symbol[53] = "\u09eb"; //5 ৫
Symbol[54] = "\u09ec"; //6 ৬
Symbol[55] = "\u09ed"; //7 ৭
Symbol[56] = "\u09ee"; //8 ৮
Symbol[57] = "\u09ef"; //9 ৯

Vowel[97] = "\u0985"; //a অ
Vowel[65] = "\u0986"; //A আ
Vowel[105] = "\u0987"; //i ই
Vowel[73] = "\u0988"; //I ঈ
Vowel[117] = "\u0989"; //u উ
Vowel[85] = "\u098a"; //U ঊ
Vowel[82] = "\u098b"; // R ঋ
Vowel[69] = "\u0990"; // E ঐ //
Vowel[101] = "\u098f"; //e এ
Vowel[79] = "\u0994"; //O ঔ
Vowel[111] = "\u0993"; // o ও


VowelCombination[0] = new Array(3);
VowelCombination[0][0] = 97; //a
VowelCombination[0][1] = 97; //a
VowelCombination[0][2] = "\u0986"; //aa আ

VowelCombination[1] = new Array(3);
VowelCombination[1][0] = 101; //e
VowelCombination[1][1] = 101; //e
VowelCombination[1][2] = "\u0988"; //ee ঈ

VowelCombination[2] = new Array(3);
VowelCombination[2][0] = 111; //o
VowelCombination[2][1] = 111; //o
VowelCombination[2][2] = "\u098a"; //oo ঊ

VowelCombination[3] = new Array(3);
VowelCombination[3][0] = 82; //R
VowelCombination[3][1] = 85; //U
VowelCombination[3][2] = "\u098b"; // R ঋ

VowelCombination[4] = new Array(3);
VowelCombination[4][0] = 97; //a
VowelCombination[4][1] = 105; //i
VowelCombination[4][2] = "\u0990"; //ai ঐ

VowelCombination[5] = new Array(3);
VowelCombination[5][0] = 97; //a
VowelCombination[5][1] = 117; //u 
VowelCombination[5][2] = "\u0994"; //au ঔ

Consonant[94] = "\u0981"; // chandrabindu
Consonant[77] = "\u0982"; // M ং
Consonant[107] = "\u0995"; //k ক
Consonant[103] = "\u0997"; //g গ
Consonant[106] = "\u099c"; //j জ
Consonant[122] = "\u099d"; //z ঝ
Consonant[84] = "\u099f"; //T ট
Consonant[68] = "\u09a1"; //D ড
Consonant[78] = "\u09a3"; //N ণ
Consonant[116] = "\u09a4"; //t ত
Consonant[100] = "\u09a6"; //d দ
Consonant[110] = "\u09a8"; //n ন
Consonant[112] = "\u09aa"; //p প
Consonant[102] = "\u09ab"; //f ফ
Consonant[98] = "\u09ac"; //b ব 
Consonant[109] = "\u09ae"; //m ম
Consonant[121] = "\u09df"; //y য়
Consonant[114] = "\u09b0"; //r র
Consonant[108] = "\u09b2"; //l ল
//Consonant[76] = "\u09b3"; //L
//Consonant[118] = "\u09b5"; //v
//Consonant[119] = "\u09b5"; //w
Consonant[115] = "\u09b8"; //s স
Consonant[104] = "\u09b9"; //h হ
Consonant[97] = ""; // just empty string
Consonant[VIRAM] = "\u09cd"; // half letter hasanta
Consonant[65] = "\u09be"; //A আ
Consonant[105] = "\u09bf"; //i ই
Consonant[73] = "\u09c0"; //I ঈ
Consonant[117] = "\u09c1"; //u উ
Consonant[85] = "\u09c2"; //U ঊ
Consonant[VRU] = "\u09c3"; // VRU Re kar
//Consonant[69] = "\u09c5"; //E
Consonant[101] = "\u09c7"; //e 	এ
//Consonant[79] = "\u09c9"; //O
Consonant[111] = "\u09cb"; //o ও



ConsonantCombination[0] = new Array(3);
ConsonantCombination[0][0] = 107; //k
ConsonantCombination[0][1] = 104; //h
ConsonantCombination[0][2] = "\u0996"; // kh খ

ConsonantCombination[1] = new Array(3);
ConsonantCombination[1][0] = 103; //g
ConsonantCombination[1][1] = 104; //h
ConsonantCombination[1][2] = "\u0998"; //gh ঘ

ConsonantCombination[2] = new Array(3);
ConsonantCombination[2][0] = 99; //c
ConsonantCombination[2][1] = 104; //h
ConsonantCombination[2][2] = "\u099a"; //ch চ

ConsonantCombination[3] = new Array(3);
ConsonantCombination[3][0] = 67; //C
ConsonantCombination[3][1] = 104; //h ছ
ConsonantCombination[3][2] = "\u099b"; //Ch

ConsonantCombination[4] = new Array(3);
ConsonantCombination[4][0] = 84; //T
ConsonantCombination[4][1] = 104; //h
ConsonantCombination[4][2] = "\u09a0"; //Th ঠ

ConsonantCombination[5] = new Array(3);
ConsonantCombination[5][0] = 68; //D
ConsonantCombination[5][1] = 104; //h
ConsonantCombination[5][2] = "\u09a2"; //Dh ঢ

ConsonantCombination[6] = new Array(3);
ConsonantCombination[6][0] = 116; //t
ConsonantCombination[6][1] = 104; //h
ConsonantCombination[6][2] = "\u09a5"; //th থ

ConsonantCombination[7] = new Array(3);
ConsonantCombination[7][0] = 100; //d
ConsonantCombination[7][1] = 104; //dh
ConsonantCombination[7][2] = "\u09a7"; //dh ধ

ConsonantCombination[8] = new Array(3);
ConsonantCombination[8][0] = 112; //p
ConsonantCombination[8][1] = 104; //ph
ConsonantCombination[8][2] = "\u09ab"; //ph ফ

ConsonantCombination[9] = new Array(3);
ConsonantCombination[9][0] = 98; //b
ConsonantCombination[9][1] = 104; //h
ConsonantCombination[9][2] = "\u09ad"; //bh ভ

ConsonantCombination[10] = new Array(3);
ConsonantCombination[10][0] = 115; //s
ConsonantCombination[10][1] = 104; //h
ConsonantCombination[10][2] = "\u09b6"; //sh শ

ConsonantCombination[11] = new Array(3);
ConsonantCombination[11][0] = 83; //S
ConsonantCombination[11][1] = 104; //h
ConsonantCombination[11][2] = "\u09b7"; //Sh ষ

ConsonantCombination[12] = new Array(3);
ConsonantCombination[12][0] = 74; //J
ConsonantCombination[12][1] = 104; //h
ConsonantCombination[12][2] = "\u099c\u09cd\u099e"; // Jh জ্ঞ

ConsonantCombination[13] = new Array(3);
ConsonantCombination[13][0] = 97; //a
ConsonantCombination[13][1] = 97; //a
ConsonantCombination[13][2] = "\u09be"; //aa আ

ConsonantCombination[14] = new Array(3);
ConsonantCombination[14][0] = 101; //e
ConsonantCombination[14][1] = 101; //e
ConsonantCombination[14][2] = "\u09c0"; //ee ঈ

ConsonantCombination[15] = new Array(3);
ConsonantCombination[15][0] = 111; //o
ConsonantCombination[15][1] = 111; //o
ConsonantCombination[15][2] = "\u09c2"; //oo ঊ

ConsonantCombination[16] = new Array(3);
ConsonantCombination[16][0] = 97; //a
ConsonantCombination[16][1] = 105; //i
ConsonantCombination[16][2] = "\u09c8"; //ai ঐ

ConsonantCombination[17] = new Array(3);
ConsonantCombination[17][0] = 97; //a
ConsonantCombination[17][1] = 117; //u
ConsonantCombination[17][2] = "\u09cc"; //au ঔ

ConsonantCombination[18] = new Array(3);
ConsonantCombination[18][0] = 78; // N
ConsonantCombination[18][1] = 71; // G
ConsonantCombination[18][2] = "\u0999"; //NG ঙ্

ConsonantCombination[19] = new Array(3);
ConsonantCombination[19][0] = 78; // N
ConsonantCombination[19][1] = 89; // Y
ConsonantCombination[19][2] = "\u099e"; //NY ঞ্

ConsonantCombination[20] = new Array(3);
ConsonantCombination[20][0] = 68; // D
ConsonantCombination[20][1] = 68; // D
ConsonantCombination[20][2] = "\u09dc"; //DD ড়্

ConsonantCombination[21] = new Array(3);
ConsonantCombination[21][0] = 68; // D
ConsonantCombination[21][1] = 72; // H
ConsonantCombination[21][2] = "\u09dd"; //DH ঢ়্


var AllCharecter = [
	["\u0020", " "],
	["\u09e6", "0"], //০
	["\u09e7", "1"], //১
	["\u09e8", "2"], //২
	["\u09e9", "3"], //৩
	["\u09ea", "4"], //৪
	["\u09eb", "5"], //৫
	["\u09ec", "6"], //৬
	["\u09ed", "7"], //৭
	["\u09ee", "8"], //৮
	["\u09ef", "9"], //৯
	["\u0985", "a"], //অ
	["\u0987", "i"], //ই
	["\u0989", "u"], //উ
	["\u098b", "R"], //ঋ
	["\u098f", "e"], //এ
	["\u0993", "o"], //ও
	["\u0986", "aa"], //আ
	["\u0988", "ee"], //ঈ
	["\u098a", "oo"], //ঊ
	["\u098b", " R"], //ঋ
	["\u0990", "ai"], //ঐ
	["\u0994", "au"], //ঔ
	["\u0982", "M"], //ং
	["\u0995", "k"], //ক
	["\u0997", "g"], //গ
	["\u099c", "j"], //জ
	["\u099d", "z"], //ঝ
	["\u099f", "T"], //ট
	["\u09a1", "D"], //ড
	["\u09a3", "N"], //ণ
	["\u09a4", "t"], //ত
	["\u09a6", "d"], //দ
	["\u09a8", "n"], //ন
	["\u09aa", "p"], //প
	["\u09ac", "b"], //ব 
	["\u09ae", "m"], //ম
	["\u09df", "y"], //য়
	["\u09b0", "r"], //র
	["\u09b2", "l"], //ল
	["\u09b8", "s"], //স
	["\u09b9", "h"], //হ
	["\u09bf", "i"], //ই
	["\u09c1", "u"], //উ
	["\u09c7", "e"], //এ
	["\u09cb", "o"], //ও
	["\u0996", "kh"], //খ
	["\u0998", "gh"], //ঘ
	["\u099a", "ch"], //চ
	["\u099b", "Ch"], //ছ
	["\u09a0", "Th"], //ঠ
	["\u09a2", "Dh"], //ঢ
	["\u09a5", "th"], //থ
	["\u09a7", "dh"], //ধ
	["\u09ab", "ph"], //ফ
	["\u09ad", "bh"], //ভ
	["\u09b6", "sh"], //শ
	["\u09b7", "Sh"], //ষ
	["\u09be", "aa"], //আ
	["\u09c0", "ee"], //ঈ
	["\u09c2", "oo"], //ঊ
	["\u09c8", "ai"], //ঐ
	["\u09cc", "au"], //ঔ
	["\u0999", "NG"], //ঙ্
	["\u099e", "NY"], //ঞ্
	["\u09dc", "DD"], //ড়্
	["\u09dd", "DH"] //ঢ়্
//"\u0983" // visarg
//"\u0986":"A" //আ
//"\u0988":"I" //ঈ
//"\u098a":"U" //ঊ
//"\u0981":" chandrabindu
//"\u09ab":"f" //ফ
//"\u09cd":" half letter
//"\u09be":"A" //আ
//"\u09c0":"I" //ঈ
//"\u09c2":"U" //ঊ
//"\u09c3": //VRU
//"\u099c\u09cd\u099e":" Jh //জ্ঞ
];
