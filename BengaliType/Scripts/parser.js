var prevkey = 32;
var hidden = false;
var posChanged = true;
var isEng = true;

var previousConsonant = false;
var previouspreviousConsonant = false;
var displace = 0;
var pos = 0;
var previousValue = "";
var keychar = "";
var prevpos = 0;


GetVowelCombination = function (key0, key1) {
    var i = 0;
    for (i = 0; i < VowelCombination.length; i++) {
        if ((VowelCombination[i][0] == key0) && (VowelCombination[i][1] == key1))
            return VowelCombination[i][2];
    }
};

GetConsonantCombination = function (key0, key1) {
    var i = 0;
    for (i = 0; i < ConsonantCombination.length; i++) {
        if ((ConsonantCombination[i][0] == key0) && (ConsonantCombination[i][1] == key1))
            return ConsonantCombination[i][2];
    }
};

positionChange = function (evt) {
    var keyCode =
    document.layers ? evt.which :
    document.all ? event.keyCode :
    document.getElementById ? evt.keyCode : 0;
    if (keyCode == 120) {
        isEng = !isEng;
        var obj;
        obj = document.getElementById("lang");
        if (obj != "undefined") {
            obj.innerHTML = "Selected Language: <b>" + (isEng ? "English" : language) + "</b>. Press F9 to change."

        }
    }
    if (keyCode >= 37 && keyCode <= 40) {
        posChanged = true;
    }
};

change = function (txtarea, evt) {
    displace = 0;
    var sent = "";
    previousValue = "";
    var e = window.event ? event : evt;
    if (e.altKey || e.ctrlKey) {
        return true;
    }
    var key = 0;
    keychar = "";
    var shift = false;
    var pos = 0;
    prevpos = 0;
    if (document.all) {
        key = e.keyCode;
    }
    else {
        key = e.which;
    }

    if ((key < 32) || (key >= 33 && key <= 47) || (key >= 59 && key <= 64) || (key >= 91 && key <= 96 && !key == 94) || (key >= 123 && key <= 127))
        return true;


    sent = ConvertChar(key);

    pos = txtarea.value.length;
    var s1 = txtarea.value.substring(0, pos + displace);
    var s2 = txtarea.value.substring(pos, txtarea.value.length);
    var scrollPos = txtarea.scrollTop;

    txtarea.value = s1 + sent + s2;

    posChanged = false;
    return false;
};
ConvertPhoneticText = function (inputText) {
    var outputText = "";
    var wordArray = inputText.split(" ");
    for (c = 0; c < wordArray.length; c++) {
        outputText += " " + ConvertPhoneticWord(wordArray[c]);
    }
    return outputText;
};

ConvertPhoneticWord = function (inputWord) {
    var outputWord = "";
    var charArray = inputWord.split("");
    for (i = 0; i < charArray.length; i++) {
        var key = charArray[i].charCodeAt(0);
        outputWord += ConvertPhoneticChar(key);
    }
    return outputWord;
};

ConvertPhoneticChar = function (key) {
    var i = 0;
    var text = "";
    for (i = 0; i < AllCharecter.length; i++) {
        if (AllCharecter[i][0].charCodeAt(0) == key) {
            text = AllCharecter[i][1];
            break;
        }
    }
    return text;
};

ConvertText = function (inputText) {
    var outputText = "";
    var wordArray = inputText.split(" ");
    for (c = 0; c < wordArray.length; c++) {
        outputText += " " + ConvertWord(wordArray[c]);
    }
    return outputText;
};

ConvertWord = function (inputWord) {
    var outputWord = "";
    inputWord = " " + inputWord + " ";
    var charArray = inputWord.split("");
    for (i = 0; i < charArray.length; i++) {
        displace = 0;
        var word = "";

        var key = charArray[i].charCodeAt(0);
        if (
			(key < 32)
			|| (key >= 33 && key <= 47)
			|| (key >= 59 && key <= 64)
			|| (key >= 91 && key <= 96 && !key == 94)
			|| (key >= 123 && key <= 127)
			|| (key > 255)
			) {
            word = String.fromCharCode(key);
        }
        else {
            word = ConvertChar(key);
        }
        pos = outputWord.length;
        var s1 = outputWord.substring(0, pos + displace);
        var s2 = outputWord.substring(pos, outputWord.length);

        outputWord = s1 + word + s2;
        posChanged = false;
    }
    return outputWord;
};

ConvertChar = function (key) {
    var text = "";
    keychar = String.fromCharCode(key);
    if (posChanged) {
        prevkey = 32;
        hidden = false;
        prevpos = 0;
        previousConsonant = false;
        previouspreviousConsonant = false;

    }
    switch (keychar) {
        case "a":
        case "e":
        case "i":
        case "o":
        case "u":
        case "A":
        case "I":
        case "U":
        case "O":
        case "E":
            var cComb = GetConsonantCombination(prevkey, key);
            var vComb = GetVowelCombination(prevkey, key);

            if (previousConsonant) {
                displace--;
                text = Consonant[key]; // nothing to append. just remove viram
            }
            else if (typeof (vComb) != "undefined") {
                if (!previouspreviousConsonant) {

                    displace--; // aa, ai, au.......
                    text = vComb;
                }
                else {
                    displace = 0;
                    text = cComb;
                }
            }
            else {
                displace = 0;
                text = Vowel[key];
            }
            previouspreviousConsonant = previousConsonant;
            previousConsonant = false;
            hidden = false;
            break;
        case "R":
            if (previousConsonant) {
                displace--; // replace the viram
                text = Consonant[VRU];
                // here no need for another U in VRU. by detecting VR it is assumed the whole word
            }
            else {
                displace = 0; // nothing to displace
                text = Vowel[key];
            }
            previouspreviousConsonant = previousConsonant;
            previousConsonant = false;
            hidden = false;
            prevkey = key;
            break;
        case "^":
        case "M":
            displace = 0;
            text = Consonant[key];
            previouspreviousConsonant = previousConsonant;
            previousConsonant = false;
            hidden = false;
            prevchar = keychar;
            break;
        case " ":
            if (previousConsonant) {
                displace--;
            }
            else {
                displace = 0;
            }
            text = Symbol[key];
            previouspreviousConsonant = previousConsonant;
            previousConsonant = false;
            hidden = false;
            prevkey = key;
            break;
        default:
            var cComb = GetConsonantCombination(prevkey, key);
            if (hidden) {
                if (typeof (cComb) != "undefined") {
                    displace = 0; // because previous word was not shown in textarea.
                    text = cComb + Consonant[VIRAM];
                    previouspreviousConsonant = previousConsonant;
                    previousConsonant = true;
                }
                else if (typeof (Consonant[key]) != "undefined") {
                    displace = 0;
                    text = Consonant[key] + Consonant[VIRAM];
                    previouspreviousConsonant = previousConsonant;
                    previousConsonant = true;
                    hidden = false;
                }
                else {
                    displace = 0;
                    previouspreviousConsonant = previousConsonant;
                    previousConsonant = false;
                }
            }
            else {
                if (typeof (cComb) != "undefined") {
                    displace -= 2; // for d, dh, D, Dh etc.
                    text = cComb + Consonant[VIRAM];
                    previouspreviousConsonant = previousConsonant;
                    previousConsonant = true;
                }
                else if (typeof (Consonant[key]) != "undefined") {
                    displace = 0;
                    text = Consonant[key] + Consonant[VIRAM]; // letter + viram
                    previouspreviousConsonant = previousConsonant;
                    previousConsonant = true;
                }
                else if (typeof (Symbol[key]) != "undefined") {
                    displace = 0;
                    text = Symbol[key];
                    previouspreviousConsonant = previousConsonant;
                    previousConsonant = false;
                }
                else {
                    previouspreviousConsonant = previousConsonant;
                    previousConsonant = true;
                    hidden = true;
                }

            }
    }

    prevkey = key;

    return text;
};

Demo = function () {
    var i = 0;
    var text = "";
    var temp = 2430;
    for (i = 0; i < 128; i++) {
        temp++;
        text += temp.toString(16) + " : " + String.fromCharCode(temp) + "<br />";
    }
    return text;
};
