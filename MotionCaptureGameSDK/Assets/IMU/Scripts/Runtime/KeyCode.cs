namespace IMU
{
    // Decompiled with JetBrains decompiler
    // Type: UnityEngine.KeyCode
    // Assembly: UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
    // MVID: CFC1AD89-0650-411E-946C-FF2E6F276711
    // Assembly location: C:\Program Files\Unity\Hub\Editor\2020.3.15f2\Editor\Data\Managed\UnityEngine\UnityEngine.CoreModule.dll

    /// <summary>
    ///   <para>Key codes returned by Event.keyCode. These map directly to a physical key on the keyboard.</para>
    /// </summary>
    /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.html">External documentation for `KeyCode`</a></footer>
    public enum KeyCode
    {
        /// <summary>
        ///   <para>Not assigned (never returned as the result of a keystroke).</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.None.html">External documentation for `KeyCode.None`</a></footer>
        None = 0,

        /// <summary>
        ///   <para>The backspace key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Backspace.html">External documentation for `KeyCode.Backspace`</a></footer>
        Backspace = 8,

        /// <summary>
        ///   <para>The tab key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Tab.html">External documentation for `KeyCode.Tab`</a></footer>
        Tab = 9,

        /// <summary>
        ///   <para>The Clear key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Clear.html">External documentation for `KeyCode.Clear`</a></footer>
        Clear = 12, // 0x0000000C

        /// <summary>
        ///   <para>Return key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Return.html">External documentation for `KeyCode.Return`</a></footer>
        Return = 13, // 0x0000000D

        /// <summary>
        ///   <para>Pause on PC machines.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Pause.html">External documentation for `KeyCode.Pause`</a></footer>
        Pause = 19, // 0x00000013

        /// <summary>
        ///   <para>Escape key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Escape.html">External documentation for `KeyCode.Escape`</a></footer>
        Escape = 27, // 0x0000001B

        /// <summary>
        ///   <para>Space key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Space.html">External documentation for `KeyCode.Space`</a></footer>
        Space = 32, // 0x00000020

        /// <summary>
        ///   <para>Exclamation mark key '!'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Exclaim.html">External documentation for `KeyCode.Exclaim`</a></footer>
        Exclaim = 33, // 0x00000021

        /// <summary>
        ///   <para>Double quote key '"'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.DoubleQuote.html">External documentation for `KeyCode.DoubleQuote`</a></footer>
        DoubleQuote = 34, // 0x00000022

        /// <summary>
        ///   <para>Hash key '#'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Hash.html">External documentation for `KeyCode.Hash`</a></footer>
        Hash = 35, // 0x00000023

        /// <summary>
        ///   <para>Dollar sign key '$'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Dollar.html">External documentation for `KeyCode.Dollar`</a></footer>
        Dollar = 36, // 0x00000024

        /// <summary>
        ///   <para>Percent '%' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Percent.html">External documentation for `KeyCode.Percent`</a></footer>
        Percent = 37, // 0x00000025

        /// <summary>
        ///   <para>Ampersand key '&amp;'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Ampersand.html">External documentation for `KeyCode.Ampersand`</a></footer>
        Ampersand = 38, // 0x00000026

        /// <summary>
        ///   <para>Quote key '.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Quote.html">External documentation for `KeyCode.Quote`</a></footer>
        Quote = 39, // 0x00000027

        /// <summary>
        ///   <para>Left Parenthesis key '('.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.LeftParen.html">External documentation for `KeyCode.LeftParen`</a></footer>
        LeftParen = 40, // 0x00000028

        /// <summary>
        ///   <para>Right Parenthesis key ')'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.RightParen.html">External documentation for `KeyCode.RightParen`</a></footer>
        RightParen = 41, // 0x00000029

        /// <summary>
        ///   <para>Asterisk key '*'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Asterisk.html">External documentation for `KeyCode.Asterisk`</a></footer>
        Asterisk = 42, // 0x0000002A

        /// <summary>
        ///   <para>Plus key '+'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Plus.html">External documentation for `KeyCode.Plus`</a></footer>
        Plus = 43, // 0x0000002B

        /// <summary>
        ///   <para>Comma ',' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Comma.html">External documentation for `KeyCode.Comma`</a></footer>
        Comma = 44, // 0x0000002C

        /// <summary>
        ///   <para>Minus '-' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Minus.html">External documentation for `KeyCode.Minus`</a></footer>
        Minus = 45, // 0x0000002D

        /// <summary>
        ///   <para>Period '.' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Period.html">External documentation for `KeyCode.Period`</a></footer>
        Period = 46, // 0x0000002E

        /// <summary>
        ///   <para>Slash '/' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Slash.html">External documentation for `KeyCode.Slash`</a></footer>
        Slash = 47, // 0x0000002F

        /// <summary>
        ///   <para>The '0' key on the top of the alphanumeric keyboard.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Alpha0.html">External documentation for `KeyCode.Alpha0`</a></footer>
        Alpha0 = 48, // 0x00000030

        /// <summary>
        ///   <para>The '1' key on the top of the alphanumeric keyboard.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Alpha1.html">External documentation for `KeyCode.Alpha1`</a></footer>
        Alpha1 = 49, // 0x00000031

        /// <summary>
        ///   <para>The '2' key on the top of the alphanumeric keyboard.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Alpha2.html">External documentation for `KeyCode.Alpha2`</a></footer>
        Alpha2 = 50, // 0x00000032

        /// <summary>
        ///   <para>The '3' key on the top of the alphanumeric keyboard.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Alpha3.html">External documentation for `KeyCode.Alpha3`</a></footer>
        Alpha3 = 51, // 0x00000033

        /// <summary>
        ///   <para>The '4' key on the top of the alphanumeric keyboard.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Alpha4.html">External documentation for `KeyCode.Alpha4`</a></footer>
        Alpha4 = 52, // 0x00000034

        /// <summary>
        ///   <para>The '5' key on the top of the alphanumeric keyboard.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Alpha5.html">External documentation for `KeyCode.Alpha5`</a></footer>
        Alpha5 = 53, // 0x00000035

        /// <summary>
        ///   <para>The '6' key on the top of the alphanumeric keyboard.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Alpha6.html">External documentation for `KeyCode.Alpha6`</a></footer>
        Alpha6 = 54, // 0x00000036

        /// <summary>
        ///   <para>The '7' key on the top of the alphanumeric keyboard.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Alpha7.html">External documentation for `KeyCode.Alpha7`</a></footer>
        Alpha7 = 55, // 0x00000037

        /// <summary>
        ///   <para>The '8' key on the top of the alphanumeric keyboard.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Alpha8.html">External documentation for `KeyCode.Alpha8`</a></footer>
        Alpha8 = 56, // 0x00000038

        /// <summary>
        ///   <para>The '9' key on the top of the alphanumeric keyboard.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Alpha9.html">External documentation for `KeyCode.Alpha9`</a></footer>
        Alpha9 = 57, // 0x00000039

        /// <summary>
        ///   <para>Colon ':' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Colon.html">External documentation for `KeyCode.Colon`</a></footer>
        Colon = 58, // 0x0000003A

        /// <summary>
        ///   <para>Semicolon ';' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Semicolon.html">External documentation for `KeyCode.Semicolon`</a></footer>
        Semicolon = 59, // 0x0000003B

        /// <summary>
        ///   <para>Less than '&lt;' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Less.html">External documentation for `KeyCode.Less`</a></footer>
        Less = 60, // 0x0000003C

        /// <summary>
        ///   <para>Equals '=' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Equals.html">External documentation for `KeyCode.Equals`</a></footer>
        Equals = 61, // 0x0000003D

        /// <summary>
        ///   <para>Greater than '&gt;' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Greater.html">External documentation for `KeyCode.Greater`</a></footer>
        Greater = 62, // 0x0000003E

        /// <summary>
        ///   <para>Question mark '?' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Question.html">External documentation for `KeyCode.Question`</a></footer>
        Question = 63, // 0x0000003F

        /// <summary>
        ///   <para>At key '@'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.At.html">External documentation for `KeyCode.At`</a></footer>
        At = 64, // 0x00000040

        /// <summary>
        ///   <para>Left square bracket key '['.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.LeftBracket.html">External documentation for `KeyCode.LeftBracket`</a></footer>
        LeftBracket = 91, // 0x0000005B

        /// <summary>
        ///   <para>Backslash key '\'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Backslash.html">External documentation for `KeyCode.Backslash`</a></footer>
        Backslash = 92, // 0x0000005C

        /// <summary>
        ///   <para>Right square bracket key ']'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.RightBracket.html">External documentation for `KeyCode.RightBracket`</a></footer>
        RightBracket = 93, // 0x0000005D

        /// <summary>
        ///   <para>Caret key '^'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Caret.html">External documentation for `KeyCode.Caret`</a></footer>
        Caret = 94, // 0x0000005E

        /// <summary>
        ///   <para>Underscore '_' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Underscore.html">External documentation for `KeyCode.Underscore`</a></footer>
        Underscore = 95, // 0x0000005F

        /// <summary>
        ///   <para>Back quote key '`'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.BackQuote.html">External documentation for `KeyCode.BackQuote`</a></footer>
        BackQuote = 96, // 0x00000060

        /// <summary>
        ///   <para>'a' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.A.html">External documentation for `KeyCode.A`</a></footer>
        A = 97, // 0x00000061

        /// <summary>
        ///   <para>'b' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.B.html">External documentation for `KeyCode.B`</a></footer>
        B = 98, // 0x00000062

        /// <summary>
        ///   <para>'c' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.C.html">External documentation for `KeyCode.C`</a></footer>
        C = 99, // 0x00000063

        /// <summary>
        ///   <para>'d' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.D.html">External documentation for `KeyCode.D`</a></footer>
        D = 100, // 0x00000064

        /// <summary>
        ///   <para>'e' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.E.html">External documentation for `KeyCode.E`</a></footer>
        E = 101, // 0x00000065

        /// <summary>
        ///   <para>'f' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.F.html">External documentation for `KeyCode.F`</a></footer>
        F = 102, // 0x00000066

        /// <summary>
        ///   <para>'g' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.G.html">External documentation for `KeyCode.G`</a></footer>
        G = 103, // 0x00000067

        /// <summary>
        ///   <para>'h' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.H.html">External documentation for `KeyCode.H`</a></footer>
        H = 104, // 0x00000068

        /// <summary>
        ///   <para>'i' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.I.html">External documentation for `KeyCode.I`</a></footer>
        I = 105, // 0x00000069

        /// <summary>
        ///   <para>'j' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.J.html">External documentation for `KeyCode.J`</a></footer>
        J = 106, // 0x0000006A

        /// <summary>
        ///   <para>'k' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.K.html">External documentation for `KeyCode.K`</a></footer>
        K = 107, // 0x0000006B

        /// <summary>
        ///   <para>'l' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.L.html">External documentation for `KeyCode.L`</a></footer>
        L = 108, // 0x0000006C

        /// <summary>
        ///   <para>'m' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.M.html">External documentation for `KeyCode.M`</a></footer>
        M = 109, // 0x0000006D

        /// <summary>
        ///   <para>'n' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.N.html">External documentation for `KeyCode.N`</a></footer>
        N = 110, // 0x0000006E

        /// <summary>
        ///   <para>'o' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.O.html">External documentation for `KeyCode.O`</a></footer>
        O = 111, // 0x0000006F

        /// <summary>
        ///   <para>'p' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.P.html">External documentation for `KeyCode.P`</a></footer>
        P = 112, // 0x00000070

        /// <summary>
        ///   <para>'q' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Q.html">External documentation for `KeyCode.Q`</a></footer>
        Q = 113, // 0x00000071

        /// <summary>
        ///   <para>'r' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.R.html">External documentation for `KeyCode.R`</a></footer>
        R = 114, // 0x00000072

        /// <summary>
        ///   <para>'s' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.S.html">External documentation for `KeyCode.S`</a></footer>
        S = 115, // 0x00000073

        /// <summary>
        ///   <para>'t' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.T.html">External documentation for `KeyCode.T`</a></footer>
        T = 116, // 0x00000074

        /// <summary>
        ///   <para>'u' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.U.html">External documentation for `KeyCode.U`</a></footer>
        U = 117, // 0x00000075

        /// <summary>
        ///   <para>'v' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.V.html">External documentation for `KeyCode.V`</a></footer>
        V = 118, // 0x00000076

        /// <summary>
        ///   <para>'w' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.W.html">External documentation for `KeyCode.W`</a></footer>
        W = 119, // 0x00000077

        /// <summary>
        ///   <para>'x' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.X.html">External documentation for `KeyCode.X`</a></footer>
        X = 120, // 0x00000078

        /// <summary>
        ///   <para>'y' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Y.html">External documentation for `KeyCode.Y`</a></footer>
        Y = 121, // 0x00000079

        /// <summary>
        ///   <para>'z' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Z.html">External documentation for `KeyCode.Z`</a></footer>
        Z = 122, // 0x0000007A

        /// <summary>
        ///   <para>Left curly bracket key '{'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.LeftCurlyBracket.html">External documentation for `KeyCode.LeftCurlyBracket`</a></footer>
        LeftCurlyBracket = 123, // 0x0000007B

        /// <summary>
        ///   <para>Pipe '|' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Pipe.html">External documentation for `KeyCode.Pipe`</a></footer>
        Pipe = 124, // 0x0000007C

        /// <summary>
        ///   <para>Right curly bracket key '}'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.RightCurlyBracket.html">External documentation for `KeyCode.RightCurlyBracket`</a></footer>
        RightCurlyBracket = 125, // 0x0000007D

        /// <summary>
        ///   <para>Tilde '~' key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Tilde.html">External documentation for `KeyCode.Tilde`</a></footer>
        Tilde = 126, // 0x0000007E

        /// <summary>
        ///   <para>The forward delete key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Delete.html">External documentation for `KeyCode.Delete`</a></footer>
        Delete = 127, // 0x0000007F

        /// <summary>
        ///   <para>Numeric keypad 0.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Keypad0.html">External documentation for `KeyCode.Keypad0`</a></footer>
        Keypad0 = 256, // 0x00000100

        /// <summary>
        ///   <para>Numeric keypad 1.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Keypad1.html">External documentation for `KeyCode.Keypad1`</a></footer>
        Keypad1 = 257, // 0x00000101

        /// <summary>
        ///   <para>Numeric keypad 2.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Keypad2.html">External documentation for `KeyCode.Keypad2`</a></footer>
        Keypad2 = 258, // 0x00000102

        /// <summary>
        ///   <para>Numeric keypad 3.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Keypad3.html">External documentation for `KeyCode.Keypad3`</a></footer>
        Keypad3 = 259, // 0x00000103

        /// <summary>
        ///   <para>Numeric keypad 4.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Keypad4.html">External documentation for `KeyCode.Keypad4`</a></footer>
        Keypad4 = 260, // 0x00000104

        /// <summary>
        ///   <para>Numeric keypad 5.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Keypad5.html">External documentation for `KeyCode.Keypad5`</a></footer>
        Keypad5 = 261, // 0x00000105

        /// <summary>
        ///   <para>Numeric keypad 6.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Keypad6.html">External documentation for `KeyCode.Keypad6`</a></footer>
        Keypad6 = 262, // 0x00000106

        /// <summary>
        ///   <para>Numeric keypad 7.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Keypad7.html">External documentation for `KeyCode.Keypad7`</a></footer>
        Keypad7 = 263, // 0x00000107

        /// <summary>
        ///   <para>Numeric keypad 8.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Keypad8.html">External documentation for `KeyCode.Keypad8`</a></footer>
        Keypad8 = 264, // 0x00000108

        /// <summary>
        ///   <para>Numeric keypad 9.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Keypad9.html">External documentation for `KeyCode.Keypad9`</a></footer>
        Keypad9 = 265, // 0x00000109

        /// <summary>
        ///   <para>Numeric keypad '.'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.KeypadPeriod.html">External documentation for `KeyCode.KeypadPeriod`</a></footer>
        KeypadPeriod = 266, // 0x0000010A

        /// <summary>
        ///   <para>Numeric keypad '/'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.KeypadDivide.html">External documentation for `KeyCode.KeypadDivide`</a></footer>
        KeypadDivide = 267, // 0x0000010B

        /// <summary>
        ///   <para>Numeric keypad '*'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.KeypadMultiply.html">External documentation for `KeyCode.KeypadMultiply`</a></footer>
        KeypadMultiply = 268, // 0x0000010C

        /// <summary>
        ///   <para>Numeric keypad '-'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.KeypadMinus.html">External documentation for `KeyCode.KeypadMinus`</a></footer>
        KeypadMinus = 269, // 0x0000010D

        /// <summary>
        ///   <para>Numeric keypad '+'.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.KeypadPlus.html">External documentation for `KeyCode.KeypadPlus`</a></footer>
        KeypadPlus = 270, // 0x0000010E

        /// <summary>
        ///   <para>Numeric keypad Enter.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.KeypadEnter.html">External documentation for `KeyCode.KeypadEnter`</a></footer>
        KeypadEnter = 271, // 0x0000010F

        /// <summary>
        ///   <para>Numeric keypad '='.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.KeypadEquals.html">External documentation for `KeyCode.KeypadEquals`</a></footer>
        KeypadEquals = 272, // 0x00000110

        /// <summary>
        ///   <para>Up arrow key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.UpArrow.html">External documentation for `KeyCode.UpArrow`</a></footer>
        UpArrow = 273, // 0x00000111

        /// <summary>
        ///   <para>Down arrow key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.DownArrow.html">External documentation for `KeyCode.DownArrow`</a></footer>
        DownArrow = 274, // 0x00000112

        /// <summary>
        ///   <para>Right arrow key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.RightArrow.html">External documentation for `KeyCode.RightArrow`</a></footer>
        RightArrow = 275, // 0x00000113

        /// <summary>
        ///   <para>Left arrow key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.LeftArrow.html">External documentation for `KeyCode.LeftArrow`</a></footer>
        LeftArrow = 276, // 0x00000114

        /// <summary>
        ///   <para>Insert key key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Insert.html">External documentation for `KeyCode.Insert`</a></footer>
        Insert = 277, // 0x00000115

        /// <summary>
        ///   <para>Home key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Home.html">External documentation for `KeyCode.Home`</a></footer>
        Home = 278, // 0x00000116

        /// <summary>
        ///   <para>End key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.End.html">External documentation for `KeyCode.End`</a></footer>
        End = 279, // 0x00000117

        /// <summary>
        ///   <para>Page up.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.PageUp.html">External documentation for `KeyCode.PageUp`</a></footer>
        PageUp = 280, // 0x00000118

        /// <summary>
        ///   <para>Page down.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.PageDown.html">External documentation for `KeyCode.PageDown`</a></footer>
        PageDown = 281, // 0x00000119

        /// <summary>
        ///   <para>F1 function key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.F1.html">External documentation for `KeyCode.F1`</a></footer>
        F1 = 282, // 0x0000011A

        /// <summary>
        ///   <para>F2 function key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.F2.html">External documentation for `KeyCode.F2`</a></footer>
        F2 = 283, // 0x0000011B

        /// <summary>
        ///   <para>F3 function key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.F3.html">External documentation for `KeyCode.F3`</a></footer>
        F3 = 284, // 0x0000011C

        /// <summary>
        ///   <para>F4 function key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.F4.html">External documentation for `KeyCode.F4`</a></footer>
        F4 = 285, // 0x0000011D

        /// <summary>
        ///   <para>F5 function key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.F5.html">External documentation for `KeyCode.F5`</a></footer>
        F5 = 286, // 0x0000011E

        /// <summary>
        ///   <para>F6 function key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.F6.html">External documentation for `KeyCode.F6`</a></footer>
        F6 = 287, // 0x0000011F

        /// <summary>
        ///   <para>F7 function key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.F7.html">External documentation for `KeyCode.F7`</a></footer>
        F7 = 288, // 0x00000120

        /// <summary>
        ///   <para>F8 function key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.F8.html">External documentation for `KeyCode.F8`</a></footer>
        F8 = 289, // 0x00000121

        /// <summary>
        ///   <para>F9 function key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.F9.html">External documentation for `KeyCode.F9`</a></footer>
        F9 = 290, // 0x00000122

        /// <summary>
        ///   <para>F10 function key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.F10.html">External documentation for `KeyCode.F10`</a></footer>
        F10 = 291, // 0x00000123

        /// <summary>
        ///   <para>F11 function key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.F11.html">External documentation for `KeyCode.F11`</a></footer>
        F11 = 292, // 0x00000124

        /// <summary>
        ///   <para>F12 function key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.F12.html">External documentation for `KeyCode.F12`</a></footer>
        F12 = 293, // 0x00000125

        /// <summary>
        ///   <para>F13 function key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.F13.html">External documentation for `KeyCode.F13`</a></footer>
        F13 = 294, // 0x00000126

        /// <summary>
        ///   <para>F14 function key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.F14.html">External documentation for `KeyCode.F14`</a></footer>
        F14 = 295, // 0x00000127

        /// <summary>
        ///   <para>F15 function key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.F15.html">External documentation for `KeyCode.F15`</a></footer>
        F15 = 296, // 0x00000128

        /// <summary>
        ///   <para>Numlock key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Numlock.html">External documentation for `KeyCode.Numlock`</a></footer>
        Numlock = 300, // 0x0000012C

        /// <summary>
        ///   <para>Capslock key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.CapsLock.html">External documentation for `KeyCode.CapsLock`</a></footer>
        CapsLock = 301, // 0x0000012D

        /// <summary>
        ///   <para>Scroll lock key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.ScrollLock.html">External documentation for `KeyCode.ScrollLock`</a></footer>
        ScrollLock = 302, // 0x0000012E

        /// <summary>
        ///   <para>Right shift key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.RightShift.html">External documentation for `KeyCode.RightShift`</a></footer>
        RightShift = 303, // 0x0000012F

        /// <summary>
        ///   <para>Left shift key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.LeftShift.html">External documentation for `KeyCode.LeftShift`</a></footer>
        LeftShift = 304, // 0x00000130

        /// <summary>
        ///   <para>Right Control key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.RightControl.html">External documentation for `KeyCode.RightControl`</a></footer>
        RightControl = 305, // 0x00000131

        /// <summary>
        ///   <para>Left Control key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.LeftControl.html">External documentation for `KeyCode.LeftControl`</a></footer>
        LeftControl = 306, // 0x00000132

        /// <summary>
        ///   <para>Right Alt key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.RightAlt.html">External documentation for `KeyCode.RightAlt`</a></footer>
        RightAlt = 307, // 0x00000133

        /// <summary>
        ///   <para>Left Alt key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.LeftAlt.html">External documentation for `KeyCode.LeftAlt`</a></footer>
        LeftAlt = 308, // 0x00000134

        /// <summary>
        ///   <para>Right Command key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.RightApple.html">External documentation for `KeyCode.RightApple`</a></footer>
        RightApple = 309, // 0x00000135

        /// <summary>
        ///   <para>Right Command key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.RightCommand.html">External documentation for `KeyCode.RightCommand`</a></footer>
        RightCommand = 309, // 0x00000135

        /// <summary>
        ///   <para>Left Command key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.LeftApple.html">External documentation for `KeyCode.LeftApple`</a></footer>
        LeftApple = 310, // 0x00000136

        /// <summary>
        ///   <para>Left Command key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.LeftCommand.html">External documentation for `KeyCode.LeftCommand`</a></footer>
        LeftCommand = 310, // 0x00000136

        /// <summary>
        ///   <para>Left Windows key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.LeftWindows.html">External documentation for `KeyCode.LeftWindows`</a></footer>
        LeftWindows = 311, // 0x00000137

        /// <summary>
        ///   <para>Right Windows key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.RightWindows.html">External documentation for `KeyCode.RightWindows`</a></footer>
        RightWindows = 312, // 0x00000138

        /// <summary>
        ///   <para>Alt Gr key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.AltGr.html">External documentation for `KeyCode.AltGr`</a></footer>
        AltGr = 313, // 0x00000139

        /// <summary>
        ///   <para>Help key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Help.html">External documentation for `KeyCode.Help`</a></footer>
        Help = 315, // 0x0000013B

        /// <summary>
        ///   <para>Print key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Print.html">External documentation for `KeyCode.Print`</a></footer>
        Print = 316, // 0x0000013C

        /// <summary>
        ///   <para>Sys Req key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.SysReq.html">External documentation for `KeyCode.SysReq`</a></footer>
        SysReq = 317, // 0x0000013D

        /// <summary>
        ///   <para>Break key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Break.html">External documentation for `KeyCode.Break`</a></footer>
        Break = 318, // 0x0000013E

        /// <summary>
        ///   <para>Menu key.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Menu.html">External documentation for `KeyCode.Menu`</a></footer>
        Menu = 319, // 0x0000013F

        /// <summary>
        ///   <para>The Left (or primary) mouse button.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Mouse0.html">External documentation for `KeyCode.Mouse0`</a></footer>
        Mouse0 = 323, // 0x00000143

        /// <summary>
        ///   <para>Right mouse button (or secondary mouse button).</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Mouse1.html">External documentation for `KeyCode.Mouse1`</a></footer>
        Mouse1 = 324, // 0x00000144

        /// <summary>
        ///   <para>Middle mouse button (or third button).</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Mouse2.html">External documentation for `KeyCode.Mouse2`</a></footer>
        Mouse2 = 325, // 0x00000145

        /// <summary>
        ///   <para>Additional (fourth) mouse button.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Mouse3.html">External documentation for `KeyCode.Mouse3`</a></footer>
        Mouse3 = 326, // 0x00000146

        /// <summary>
        ///   <para>Additional (fifth) mouse button.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Mouse4.html">External documentation for `KeyCode.Mouse4`</a></footer>
        Mouse4 = 327, // 0x00000147

        /// <summary>
        ///   <para>Additional (or sixth) mouse button.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Mouse5.html">External documentation for `KeyCode.Mouse5`</a></footer>
        Mouse5 = 328, // 0x00000148

        /// <summary>
        ///   <para>Additional (or seventh) mouse button.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Mouse6.html">External documentation for `KeyCode.Mouse6`</a></footer>
        Mouse6 = 329, // 0x00000149

        /// <summary>
        ///   <para>Button 0 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton0.html">External documentation for `KeyCode.JoystickButton0`</a></footer>
        JoystickButton0 = 330, // 0x0000014A

        /// <summary>
        ///   <para>Button 1 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton1.html">External documentation for `KeyCode.JoystickButton1`</a></footer>
        JoystickButton1 = 331, // 0x0000014B

        /// <summary>
        ///   <para>Button 2 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton2.html">External documentation for `KeyCode.JoystickButton2`</a></footer>
        JoystickButton2 = 332, // 0x0000014C

        /// <summary>
        ///   <para>Button 3 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton3.html">External documentation for `KeyCode.JoystickButton3`</a></footer>
        JoystickButton3 = 333, // 0x0000014D

        /// <summary>
        ///   <para>Button 4 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton4.html">External documentation for `KeyCode.JoystickButton4`</a></footer>
        JoystickButton4 = 334, // 0x0000014E

        /// <summary>
        ///   <para>Button 5 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton5.html">External documentation for `KeyCode.JoystickButton5`</a></footer>
        JoystickButton5 = 335, // 0x0000014F

        /// <summary>
        ///   <para>Button 6 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton6.html">External documentation for `KeyCode.JoystickButton6`</a></footer>
        JoystickButton6 = 336, // 0x00000150

        /// <summary>
        ///   <para>Button 7 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton7.html">External documentation for `KeyCode.JoystickButton7`</a></footer>
        JoystickButton7 = 337, // 0x00000151

        /// <summary>
        ///   <para>Button 8 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton8.html">External documentation for `KeyCode.JoystickButton8`</a></footer>
        JoystickButton8 = 338, // 0x00000152

        /// <summary>
        ///   <para>Button 9 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton9.html">External documentation for `KeyCode.JoystickButton9`</a></footer>
        JoystickButton9 = 339, // 0x00000153

        /// <summary>
        ///   <para>Button 10 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton10.html">External documentation for `KeyCode.JoystickButton10`</a></footer>
        JoystickButton10 = 340, // 0x00000154

        /// <summary>
        ///   <para>Button 11 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton11.html">External documentation for `KeyCode.JoystickButton11`</a></footer>
        JoystickButton11 = 341, // 0x00000155

        /// <summary>
        ///   <para>Button 12 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton12.html">External documentation for `KeyCode.JoystickButton12`</a></footer>
        JoystickButton12 = 342, // 0x00000156

        /// <summary>
        ///   <para>Button 13 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton13.html">External documentation for `KeyCode.JoystickButton13`</a></footer>
        JoystickButton13 = 343, // 0x00000157

        /// <summary>
        ///   <para>Button 14 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton14.html">External documentation for `KeyCode.JoystickButton14`</a></footer>
        JoystickButton14 = 344, // 0x00000158

        /// <summary>
        ///   <para>Button 15 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton15.html">External documentation for `KeyCode.JoystickButton15`</a></footer>
        JoystickButton15 = 345, // 0x00000159

        /// <summary>
        ///   <para>Button 16 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton16.html">External documentation for `KeyCode.JoystickButton16`</a></footer>
        JoystickButton16 = 346, // 0x0000015A

        /// <summary>
        ///   <para>Button 17 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton17.html">External documentation for `KeyCode.JoystickButton17`</a></footer>
        JoystickButton17 = 347, // 0x0000015B

        /// <summary>
        ///   <para>Button 18 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton18.html">External documentation for `KeyCode.JoystickButton18`</a></footer>
        JoystickButton18 = 348, // 0x0000015C

        /// <summary>
        ///   <para>Button 19 on any joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.JoystickButton19.html">External documentation for `KeyCode.JoystickButton19`</a></footer>
        JoystickButton19 = 349, // 0x0000015D

        /// <summary>
        ///   <para>Button 0 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button0.html">External documentation for `KeyCode.Joystick1Button0`</a></footer>
        Joystick1Button0 = 350, // 0x0000015E

        /// <summary>
        ///   <para>Button 1 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button1.html">External documentation for `KeyCode.Joystick1Button1`</a></footer>
        Joystick1Button1 = 351, // 0x0000015F

        /// <summary>
        ///   <para>Button 2 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button2.html">External documentation for `KeyCode.Joystick1Button2`</a></footer>
        Joystick1Button2 = 352, // 0x00000160

        /// <summary>
        ///   <para>Button 3 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button3.html">External documentation for `KeyCode.Joystick1Button3`</a></footer>
        Joystick1Button3 = 353, // 0x00000161

        /// <summary>
        ///   <para>Button 4 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button4.html">External documentation for `KeyCode.Joystick1Button4`</a></footer>
        Joystick1Button4 = 354, // 0x00000162

        /// <summary>
        ///   <para>Button 5 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button5.html">External documentation for `KeyCode.Joystick1Button5`</a></footer>
        Joystick1Button5 = 355, // 0x00000163

        /// <summary>
        ///   <para>Button 6 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button6.html">External documentation for `KeyCode.Joystick1Button6`</a></footer>
        Joystick1Button6 = 356, // 0x00000164

        /// <summary>
        ///   <para>Button 7 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button7.html">External documentation for `KeyCode.Joystick1Button7`</a></footer>
        Joystick1Button7 = 357, // 0x00000165

        /// <summary>
        ///   <para>Button 8 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button8.html">External documentation for `KeyCode.Joystick1Button8`</a></footer>
        Joystick1Button8 = 358, // 0x00000166

        /// <summary>
        ///   <para>Button 9 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button9.html">External documentation for `KeyCode.Joystick1Button9`</a></footer>
        Joystick1Button9 = 359, // 0x00000167

        /// <summary>
        ///   <para>Button 10 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button10.html">External documentation for `KeyCode.Joystick1Button10`</a></footer>
        Joystick1Button10 = 360, // 0x00000168

        /// <summary>
        ///   <para>Button 11 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button11.html">External documentation for `KeyCode.Joystick1Button11`</a></footer>
        Joystick1Button11 = 361, // 0x00000169

        /// <summary>
        ///   <para>Button 12 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button12.html">External documentation for `KeyCode.Joystick1Button12`</a></footer>
        Joystick1Button12 = 362, // 0x0000016A

        /// <summary>
        ///   <para>Button 13 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button13.html">External documentation for `KeyCode.Joystick1Button13`</a></footer>
        Joystick1Button13 = 363, // 0x0000016B

        /// <summary>
        ///   <para>Button 14 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button14.html">External documentation for `KeyCode.Joystick1Button14`</a></footer>
        Joystick1Button14 = 364, // 0x0000016C

        /// <summary>
        ///   <para>Button 15 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button15.html">External documentation for `KeyCode.Joystick1Button15`</a></footer>
        Joystick1Button15 = 365, // 0x0000016D

        /// <summary>
        ///   <para>Button 16 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button16.html">External documentation for `KeyCode.Joystick1Button16`</a></footer>
        Joystick1Button16 = 366, // 0x0000016E

        /// <summary>
        ///   <para>Button 17 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button17.html">External documentation for `KeyCode.Joystick1Button17`</a></footer>
        Joystick1Button17 = 367, // 0x0000016F

        /// <summary>
        ///   <para>Button 18 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button18.html">External documentation for `KeyCode.Joystick1Button18`</a></footer>
        Joystick1Button18 = 368, // 0x00000170

        /// <summary>
        ///   <para>Button 19 on first joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick1Button19.html">External documentation for `KeyCode.Joystick1Button19`</a></footer>
        Joystick1Button19 = 369, // 0x00000171

        /// <summary>
        ///   <para>Button 0 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button0.html">External documentation for `KeyCode.Joystick2Button0`</a></footer>
        Joystick2Button0 = 370, // 0x00000172

        /// <summary>
        ///   <para>Button 1 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button1.html">External documentation for `KeyCode.Joystick2Button1`</a></footer>
        Joystick2Button1 = 371, // 0x00000173

        /// <summary>
        ///   <para>Button 2 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button2.html">External documentation for `KeyCode.Joystick2Button2`</a></footer>
        Joystick2Button2 = 372, // 0x00000174

        /// <summary>
        ///   <para>Button 3 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button3.html">External documentation for `KeyCode.Joystick2Button3`</a></footer>
        Joystick2Button3 = 373, // 0x00000175

        /// <summary>
        ///   <para>Button 4 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button4.html">External documentation for `KeyCode.Joystick2Button4`</a></footer>
        Joystick2Button4 = 374, // 0x00000176

        /// <summary>
        ///   <para>Button 5 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button5.html">External documentation for `KeyCode.Joystick2Button5`</a></footer>
        Joystick2Button5 = 375, // 0x00000177

        /// <summary>
        ///   <para>Button 6 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button6.html">External documentation for `KeyCode.Joystick2Button6`</a></footer>
        Joystick2Button6 = 376, // 0x00000178

        /// <summary>
        ///   <para>Button 7 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button7.html">External documentation for `KeyCode.Joystick2Button7`</a></footer>
        Joystick2Button7 = 377, // 0x00000179

        /// <summary>
        ///   <para>Button 8 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button8.html">External documentation for `KeyCode.Joystick2Button8`</a></footer>
        Joystick2Button8 = 378, // 0x0000017A

        /// <summary>
        ///   <para>Button 9 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button9.html">External documentation for `KeyCode.Joystick2Button9`</a></footer>
        Joystick2Button9 = 379, // 0x0000017B

        /// <summary>
        ///   <para>Button 10 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button10.html">External documentation for `KeyCode.Joystick2Button10`</a></footer>
        Joystick2Button10 = 380, // 0x0000017C

        /// <summary>
        ///   <para>Button 11 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button11.html">External documentation for `KeyCode.Joystick2Button11`</a></footer>
        Joystick2Button11 = 381, // 0x0000017D

        /// <summary>
        ///   <para>Button 12 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button12.html">External documentation for `KeyCode.Joystick2Button12`</a></footer>
        Joystick2Button12 = 382, // 0x0000017E

        /// <summary>
        ///   <para>Button 13 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button13.html">External documentation for `KeyCode.Joystick2Button13`</a></footer>
        Joystick2Button13 = 383, // 0x0000017F

        /// <summary>
        ///   <para>Button 14 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button14.html">External documentation for `KeyCode.Joystick2Button14`</a></footer>
        Joystick2Button14 = 384, // 0x00000180

        /// <summary>
        ///   <para>Button 15 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button15.html">External documentation for `KeyCode.Joystick2Button15`</a></footer>
        Joystick2Button15 = 385, // 0x00000181

        /// <summary>
        ///   <para>Button 16 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button16.html">External documentation for `KeyCode.Joystick2Button16`</a></footer>
        Joystick2Button16 = 386, // 0x00000182

        /// <summary>
        ///   <para>Button 17 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button17.html">External documentation for `KeyCode.Joystick2Button17`</a></footer>
        Joystick2Button17 = 387, // 0x00000183

        /// <summary>
        ///   <para>Button 18 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button18.html">External documentation for `KeyCode.Joystick2Button18`</a></footer>
        Joystick2Button18 = 388, // 0x00000184

        /// <summary>
        ///   <para>Button 19 on second joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick2Button19.html">External documentation for `KeyCode.Joystick2Button19`</a></footer>
        Joystick2Button19 = 389, // 0x00000185

        /// <summary>
        ///   <para>Button 0 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button0.html">External documentation for `KeyCode.Joystick3Button0`</a></footer>
        Joystick3Button0 = 390, // 0x00000186

        /// <summary>
        ///   <para>Button 1 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button1.html">External documentation for `KeyCode.Joystick3Button1`</a></footer>
        Joystick3Button1 = 391, // 0x00000187

        /// <summary>
        ///   <para>Button 2 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button2.html">External documentation for `KeyCode.Joystick3Button2`</a></footer>
        Joystick3Button2 = 392, // 0x00000188

        /// <summary>
        ///   <para>Button 3 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button3.html">External documentation for `KeyCode.Joystick3Button3`</a></footer>
        Joystick3Button3 = 393, // 0x00000189

        /// <summary>
        ///   <para>Button 4 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button4.html">External documentation for `KeyCode.Joystick3Button4`</a></footer>
        Joystick3Button4 = 394, // 0x0000018A

        /// <summary>
        ///   <para>Button 5 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button5.html">External documentation for `KeyCode.Joystick3Button5`</a></footer>
        Joystick3Button5 = 395, // 0x0000018B

        /// <summary>
        ///   <para>Button 6 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button6.html">External documentation for `KeyCode.Joystick3Button6`</a></footer>
        Joystick3Button6 = 396, // 0x0000018C

        /// <summary>
        ///   <para>Button 7 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button7.html">External documentation for `KeyCode.Joystick3Button7`</a></footer>
        Joystick3Button7 = 397, // 0x0000018D

        /// <summary>
        ///   <para>Button 8 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button8.html">External documentation for `KeyCode.Joystick3Button8`</a></footer>
        Joystick3Button8 = 398, // 0x0000018E

        /// <summary>
        ///   <para>Button 9 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button9.html">External documentation for `KeyCode.Joystick3Button9`</a></footer>
        Joystick3Button9 = 399, // 0x0000018F

        /// <summary>
        ///   <para>Button 10 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button10.html">External documentation for `KeyCode.Joystick3Button10`</a></footer>
        Joystick3Button10 = 400, // 0x00000190

        /// <summary>
        ///   <para>Button 11 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button11.html">External documentation for `KeyCode.Joystick3Button11`</a></footer>
        Joystick3Button11 = 401, // 0x00000191

        /// <summary>
        ///   <para>Button 12 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button12.html">External documentation for `KeyCode.Joystick3Button12`</a></footer>
        Joystick3Button12 = 402, // 0x00000192

        /// <summary>
        ///   <para>Button 13 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button13.html">External documentation for `KeyCode.Joystick3Button13`</a></footer>
        Joystick3Button13 = 403, // 0x00000193

        /// <summary>
        ///   <para>Button 14 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button14.html">External documentation for `KeyCode.Joystick3Button14`</a></footer>
        Joystick3Button14 = 404, // 0x00000194

        /// <summary>
        ///   <para>Button 15 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button15.html">External documentation for `KeyCode.Joystick3Button15`</a></footer>
        Joystick3Button15 = 405, // 0x00000195

        /// <summary>
        ///   <para>Button 16 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button16.html">External documentation for `KeyCode.Joystick3Button16`</a></footer>
        Joystick3Button16 = 406, // 0x00000196

        /// <summary>
        ///   <para>Button 17 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button17.html">External documentation for `KeyCode.Joystick3Button17`</a></footer>
        Joystick3Button17 = 407, // 0x00000197

        /// <summary>
        ///   <para>Button 18 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button18.html">External documentation for `KeyCode.Joystick3Button18`</a></footer>
        Joystick3Button18 = 408, // 0x00000198

        /// <summary>
        ///   <para>Button 19 on third joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick3Button19.html">External documentation for `KeyCode.Joystick3Button19`</a></footer>
        Joystick3Button19 = 409, // 0x00000199

        /// <summary>
        ///   <para>Button 0 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button0.html">External documentation for `KeyCode.Joystick4Button0`</a></footer>
        Joystick4Button0 = 410, // 0x0000019A

        /// <summary>
        ///   <para>Button 1 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button1.html">External documentation for `KeyCode.Joystick4Button1`</a></footer>
        Joystick4Button1 = 411, // 0x0000019B

        /// <summary>
        ///   <para>Button 2 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button2.html">External documentation for `KeyCode.Joystick4Button2`</a></footer>
        Joystick4Button2 = 412, // 0x0000019C

        /// <summary>
        ///   <para>Button 3 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button3.html">External documentation for `KeyCode.Joystick4Button3`</a></footer>
        Joystick4Button3 = 413, // 0x0000019D

        /// <summary>
        ///   <para>Button 4 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button4.html">External documentation for `KeyCode.Joystick4Button4`</a></footer>
        Joystick4Button4 = 414, // 0x0000019E

        /// <summary>
        ///   <para>Button 5 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button5.html">External documentation for `KeyCode.Joystick4Button5`</a></footer>
        Joystick4Button5 = 415, // 0x0000019F

        /// <summary>
        ///   <para>Button 6 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button6.html">External documentation for `KeyCode.Joystick4Button6`</a></footer>
        Joystick4Button6 = 416, // 0x000001A0

        /// <summary>
        ///   <para>Button 7 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button7.html">External documentation for `KeyCode.Joystick4Button7`</a></footer>
        Joystick4Button7 = 417, // 0x000001A1

        /// <summary>
        ///   <para>Button 8 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button8.html">External documentation for `KeyCode.Joystick4Button8`</a></footer>
        Joystick4Button8 = 418, // 0x000001A2

        /// <summary>
        ///   <para>Button 9 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button9.html">External documentation for `KeyCode.Joystick4Button9`</a></footer>
        Joystick4Button9 = 419, // 0x000001A3

        /// <summary>
        ///   <para>Button 10 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button10.html">External documentation for `KeyCode.Joystick4Button10`</a></footer>
        Joystick4Button10 = 420, // 0x000001A4

        /// <summary>
        ///   <para>Button 11 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button11.html">External documentation for `KeyCode.Joystick4Button11`</a></footer>
        Joystick4Button11 = 421, // 0x000001A5

        /// <summary>
        ///   <para>Button 12 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button12.html">External documentation for `KeyCode.Joystick4Button12`</a></footer>
        Joystick4Button12 = 422, // 0x000001A6

        /// <summary>
        ///   <para>Button 13 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button13.html">External documentation for `KeyCode.Joystick4Button13`</a></footer>
        Joystick4Button13 = 423, // 0x000001A7

        /// <summary>
        ///   <para>Button 14 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button14.html">External documentation for `KeyCode.Joystick4Button14`</a></footer>
        Joystick4Button14 = 424, // 0x000001A8

        /// <summary>
        ///   <para>Button 15 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button15.html">External documentation for `KeyCode.Joystick4Button15`</a></footer>
        Joystick4Button15 = 425, // 0x000001A9

        /// <summary>
        ///   <para>Button 16 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button16.html">External documentation for `KeyCode.Joystick4Button16`</a></footer>
        Joystick4Button16 = 426, // 0x000001AA

        /// <summary>
        ///   <para>Button 17 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button17.html">External documentation for `KeyCode.Joystick4Button17`</a></footer>
        Joystick4Button17 = 427, // 0x000001AB

        /// <summary>
        ///   <para>Button 18 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button18.html">External documentation for `KeyCode.Joystick4Button18`</a></footer>
        Joystick4Button18 = 428, // 0x000001AC

        /// <summary>
        ///   <para>Button 19 on forth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick4Button19.html">External documentation for `KeyCode.Joystick4Button19`</a></footer>
        Joystick4Button19 = 429, // 0x000001AD

        /// <summary>
        ///   <para>Button 0 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button0.html">External documentation for `KeyCode.Joystick5Button0`</a></footer>
        Joystick5Button0 = 430, // 0x000001AE

        /// <summary>
        ///   <para>Button 1 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button1.html">External documentation for `KeyCode.Joystick5Button1`</a></footer>
        Joystick5Button1 = 431, // 0x000001AF

        /// <summary>
        ///   <para>Button 2 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button2.html">External documentation for `KeyCode.Joystick5Button2`</a></footer>
        Joystick5Button2 = 432, // 0x000001B0

        /// <summary>
        ///   <para>Button 3 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button3.html">External documentation for `KeyCode.Joystick5Button3`</a></footer>
        Joystick5Button3 = 433, // 0x000001B1

        /// <summary>
        ///   <para>Button 4 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button4.html">External documentation for `KeyCode.Joystick5Button4`</a></footer>
        Joystick5Button4 = 434, // 0x000001B2

        /// <summary>
        ///   <para>Button 5 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button5.html">External documentation for `KeyCode.Joystick5Button5`</a></footer>
        Joystick5Button5 = 435, // 0x000001B3

        /// <summary>
        ///   <para>Button 6 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button6.html">External documentation for `KeyCode.Joystick5Button6`</a></footer>
        Joystick5Button6 = 436, // 0x000001B4

        /// <summary>
        ///   <para>Button 7 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button7.html">External documentation for `KeyCode.Joystick5Button7`</a></footer>
        Joystick5Button7 = 437, // 0x000001B5

        /// <summary>
        ///   <para>Button 8 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button8.html">External documentation for `KeyCode.Joystick5Button8`</a></footer>
        Joystick5Button8 = 438, // 0x000001B6

        /// <summary>
        ///   <para>Button 9 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button9.html">External documentation for `KeyCode.Joystick5Button9`</a></footer>
        Joystick5Button9 = 439, // 0x000001B7

        /// <summary>
        ///   <para>Button 10 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button10.html">External documentation for `KeyCode.Joystick5Button10`</a></footer>
        Joystick5Button10 = 440, // 0x000001B8

        /// <summary>
        ///   <para>Button 11 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button11.html">External documentation for `KeyCode.Joystick5Button11`</a></footer>
        Joystick5Button11 = 441, // 0x000001B9

        /// <summary>
        ///   <para>Button 12 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button12.html">External documentation for `KeyCode.Joystick5Button12`</a></footer>
        Joystick5Button12 = 442, // 0x000001BA

        /// <summary>
        ///   <para>Button 13 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button13.html">External documentation for `KeyCode.Joystick5Button13`</a></footer>
        Joystick5Button13 = 443, // 0x000001BB

        /// <summary>
        ///   <para>Button 14 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button14.html">External documentation for `KeyCode.Joystick5Button14`</a></footer>
        Joystick5Button14 = 444, // 0x000001BC

        /// <summary>
        ///   <para>Button 15 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button15.html">External documentation for `KeyCode.Joystick5Button15`</a></footer>
        Joystick5Button15 = 445, // 0x000001BD

        /// <summary>
        ///   <para>Button 16 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button16.html">External documentation for `KeyCode.Joystick5Button16`</a></footer>
        Joystick5Button16 = 446, // 0x000001BE

        /// <summary>
        ///   <para>Button 17 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button17.html">External documentation for `KeyCode.Joystick5Button17`</a></footer>
        Joystick5Button17 = 447, // 0x000001BF

        /// <summary>
        ///   <para>Button 18 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button18.html">External documentation for `KeyCode.Joystick5Button18`</a></footer>
        Joystick5Button18 = 448, // 0x000001C0

        /// <summary>
        ///   <para>Button 19 on fifth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick5Button19.html">External documentation for `KeyCode.Joystick5Button19`</a></footer>
        Joystick5Button19 = 449, // 0x000001C1

        /// <summary>
        ///   <para>Button 0 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button0.html">External documentation for `KeyCode.Joystick6Button0`</a></footer>
        Joystick6Button0 = 450, // 0x000001C2

        /// <summary>
        ///   <para>Button 1 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button1.html">External documentation for `KeyCode.Joystick6Button1`</a></footer>
        Joystick6Button1 = 451, // 0x000001C3

        /// <summary>
        ///   <para>Button 2 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button2.html">External documentation for `KeyCode.Joystick6Button2`</a></footer>
        Joystick6Button2 = 452, // 0x000001C4

        /// <summary>
        ///   <para>Button 3 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button3.html">External documentation for `KeyCode.Joystick6Button3`</a></footer>
        Joystick6Button3 = 453, // 0x000001C5

        /// <summary>
        ///   <para>Button 4 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button4.html">External documentation for `KeyCode.Joystick6Button4`</a></footer>
        Joystick6Button4 = 454, // 0x000001C6

        /// <summary>
        ///   <para>Button 5 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button5.html">External documentation for `KeyCode.Joystick6Button5`</a></footer>
        Joystick6Button5 = 455, // 0x000001C7

        /// <summary>
        ///   <para>Button 6 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button6.html">External documentation for `KeyCode.Joystick6Button6`</a></footer>
        Joystick6Button6 = 456, // 0x000001C8

        /// <summary>
        ///   <para>Button 7 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button7.html">External documentation for `KeyCode.Joystick6Button7`</a></footer>
        Joystick6Button7 = 457, // 0x000001C9

        /// <summary>
        ///   <para>Button 8 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button8.html">External documentation for `KeyCode.Joystick6Button8`</a></footer>
        Joystick6Button8 = 458, // 0x000001CA

        /// <summary>
        ///   <para>Button 9 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button9.html">External documentation for `KeyCode.Joystick6Button9`</a></footer>
        Joystick6Button9 = 459, // 0x000001CB

        /// <summary>
        ///   <para>Button 10 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button10.html">External documentation for `KeyCode.Joystick6Button10`</a></footer>
        Joystick6Button10 = 460, // 0x000001CC

        /// <summary>
        ///   <para>Button 11 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button11.html">External documentation for `KeyCode.Joystick6Button11`</a></footer>
        Joystick6Button11 = 461, // 0x000001CD

        /// <summary>
        ///   <para>Button 12 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button12.html">External documentation for `KeyCode.Joystick6Button12`</a></footer>
        Joystick6Button12 = 462, // 0x000001CE

        /// <summary>
        ///   <para>Button 13 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button13.html">External documentation for `KeyCode.Joystick6Button13`</a></footer>
        Joystick6Button13 = 463, // 0x000001CF

        /// <summary>
        ///   <para>Button 14 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button14.html">External documentation for `KeyCode.Joystick6Button14`</a></footer>
        Joystick6Button14 = 464, // 0x000001D0

        /// <summary>
        ///   <para>Button 15 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button15.html">External documentation for `KeyCode.Joystick6Button15`</a></footer>
        Joystick6Button15 = 465, // 0x000001D1

        /// <summary>
        ///   <para>Button 16 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button16.html">External documentation for `KeyCode.Joystick6Button16`</a></footer>
        Joystick6Button16 = 466, // 0x000001D2

        /// <summary>
        ///   <para>Button 17 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button17.html">External documentation for `KeyCode.Joystick6Button17`</a></footer>
        Joystick6Button17 = 467, // 0x000001D3

        /// <summary>
        ///   <para>Button 18 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button18.html">External documentation for `KeyCode.Joystick6Button18`</a></footer>
        Joystick6Button18 = 468, // 0x000001D4

        /// <summary>
        ///   <para>Button 19 on sixth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick6Button19.html">External documentation for `KeyCode.Joystick6Button19`</a></footer>
        Joystick6Button19 = 469, // 0x000001D5

        /// <summary>
        ///   <para>Button 0 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button0.html">External documentation for `KeyCode.Joystick7Button0`</a></footer>
        Joystick7Button0 = 470, // 0x000001D6

        /// <summary>
        ///   <para>Button 1 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button1.html">External documentation for `KeyCode.Joystick7Button1`</a></footer>
        Joystick7Button1 = 471, // 0x000001D7

        /// <summary>
        ///   <para>Button 2 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button2.html">External documentation for `KeyCode.Joystick7Button2`</a></footer>
        Joystick7Button2 = 472, // 0x000001D8

        /// <summary>
        ///   <para>Button 3 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button3.html">External documentation for `KeyCode.Joystick7Button3`</a></footer>
        Joystick7Button3 = 473, // 0x000001D9

        /// <summary>
        ///   <para>Button 4 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button4.html">External documentation for `KeyCode.Joystick7Button4`</a></footer>
        Joystick7Button4 = 474, // 0x000001DA

        /// <summary>
        ///   <para>Button 5 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button5.html">External documentation for `KeyCode.Joystick7Button5`</a></footer>
        Joystick7Button5 = 475, // 0x000001DB

        /// <summary>
        ///   <para>Button 6 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button6.html">External documentation for `KeyCode.Joystick7Button6`</a></footer>
        Joystick7Button6 = 476, // 0x000001DC

        /// <summary>
        ///   <para>Button 7 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button7.html">External documentation for `KeyCode.Joystick7Button7`</a></footer>
        Joystick7Button7 = 477, // 0x000001DD

        /// <summary>
        ///   <para>Button 8 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button8.html">External documentation for `KeyCode.Joystick7Button8`</a></footer>
        Joystick7Button8 = 478, // 0x000001DE

        /// <summary>
        ///   <para>Button 9 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button9.html">External documentation for `KeyCode.Joystick7Button9`</a></footer>
        Joystick7Button9 = 479, // 0x000001DF

        /// <summary>
        ///   <para>Button 10 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button10.html">External documentation for `KeyCode.Joystick7Button10`</a></footer>
        Joystick7Button10 = 480, // 0x000001E0

        /// <summary>
        ///   <para>Button 11 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button11.html">External documentation for `KeyCode.Joystick7Button11`</a></footer>
        Joystick7Button11 = 481, // 0x000001E1

        /// <summary>
        ///   <para>Button 12 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button12.html">External documentation for `KeyCode.Joystick7Button12`</a></footer>
        Joystick7Button12 = 482, // 0x000001E2

        /// <summary>
        ///   <para>Button 13 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button13.html">External documentation for `KeyCode.Joystick7Button13`</a></footer>
        Joystick7Button13 = 483, // 0x000001E3

        /// <summary>
        ///   <para>Button 14 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button14.html">External documentation for `KeyCode.Joystick7Button14`</a></footer>
        Joystick7Button14 = 484, // 0x000001E4

        /// <summary>
        ///   <para>Button 15 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button15.html">External documentation for `KeyCode.Joystick7Button15`</a></footer>
        Joystick7Button15 = 485, // 0x000001E5

        /// <summary>
        ///   <para>Button 16 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button16.html">External documentation for `KeyCode.Joystick7Button16`</a></footer>
        Joystick7Button16 = 486, // 0x000001E6

        /// <summary>
        ///   <para>Button 17 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button17.html">External documentation for `KeyCode.Joystick7Button17`</a></footer>
        Joystick7Button17 = 487, // 0x000001E7

        /// <summary>
        ///   <para>Button 18 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button18.html">External documentation for `KeyCode.Joystick7Button18`</a></footer>
        Joystick7Button18 = 488, // 0x000001E8

        /// <summary>
        ///   <para>Button 19 on seventh joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick7Button19.html">External documentation for `KeyCode.Joystick7Button19`</a></footer>
        Joystick7Button19 = 489, // 0x000001E9

        /// <summary>
        ///   <para>Button 0 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button0.html">External documentation for `KeyCode.Joystick8Button0`</a></footer>
        Joystick8Button0 = 490, // 0x000001EA

        /// <summary>
        ///   <para>Button 1 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button1.html">External documentation for `KeyCode.Joystick8Button1`</a></footer>
        Joystick8Button1 = 491, // 0x000001EB

        /// <summary>
        ///   <para>Button 2 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button2.html">External documentation for `KeyCode.Joystick8Button2`</a></footer>
        Joystick8Button2 = 492, // 0x000001EC

        /// <summary>
        ///   <para>Button 3 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button3.html">External documentation for `KeyCode.Joystick8Button3`</a></footer>
        Joystick8Button3 = 493, // 0x000001ED

        /// <summary>
        ///   <para>Button 4 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button4.html">External documentation for `KeyCode.Joystick8Button4`</a></footer>
        Joystick8Button4 = 494, // 0x000001EE

        /// <summary>
        ///   <para>Button 5 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button5.html">External documentation for `KeyCode.Joystick8Button5`</a></footer>
        Joystick8Button5 = 495, // 0x000001EF

        /// <summary>
        ///   <para>Button 6 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button6.html">External documentation for `KeyCode.Joystick8Button6`</a></footer>
        Joystick8Button6 = 496, // 0x000001F0

        /// <summary>
        ///   <para>Button 7 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button7.html">External documentation for `KeyCode.Joystick8Button7`</a></footer>
        Joystick8Button7 = 497, // 0x000001F1

        /// <summary>
        ///   <para>Button 8 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button8.html">External documentation for `KeyCode.Joystick8Button8`</a></footer>
        Joystick8Button8 = 498, // 0x000001F2

        /// <summary>
        ///   <para>Button 9 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button9.html">External documentation for `KeyCode.Joystick8Button9`</a></footer>
        Joystick8Button9 = 499, // 0x000001F3

        /// <summary>
        ///   <para>Button 10 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button10.html">External documentation for `KeyCode.Joystick8Button10`</a></footer>
        Joystick8Button10 = 500, // 0x000001F4

        /// <summary>
        ///   <para>Button 11 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button11.html">External documentation for `KeyCode.Joystick8Button11`</a></footer>
        Joystick8Button11 = 501, // 0x000001F5

        /// <summary>
        ///   <para>Button 12 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button12.html">External documentation for `KeyCode.Joystick8Button12`</a></footer>
        Joystick8Button12 = 502, // 0x000001F6

        /// <summary>
        ///   <para>Button 13 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button13.html">External documentation for `KeyCode.Joystick8Button13`</a></footer>
        Joystick8Button13 = 503, // 0x000001F7

        /// <summary>
        ///   <para>Button 14 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button14.html">External documentation for `KeyCode.Joystick8Button14`</a></footer>
        Joystick8Button14 = 504, // 0x000001F8

        /// <summary>
        ///   <para>Button 15 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button15.html">External documentation for `KeyCode.Joystick8Button15`</a></footer>
        Joystick8Button15 = 505, // 0x000001F9

        /// <summary>
        ///   <para>Button 16 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button16.html">External documentation for `KeyCode.Joystick8Button16`</a></footer>
        Joystick8Button16 = 506, // 0x000001FA

        /// <summary>
        ///   <para>Button 17 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button17.html">External documentation for `KeyCode.Joystick8Button17`</a></footer>
        Joystick8Button17 = 507, // 0x000001FB

        /// <summary>
        ///   <para>Button 18 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button18.html">External documentation for `KeyCode.Joystick8Button18`</a></footer>
        Joystick8Button18 = 508, // 0x000001FC

        /// <summary>
        ///   <para>Button 19 on eighth joystick.</para>
        /// </summary>
        /// <footer><a href="file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.15f2/Editor/Data/Documentation/en/ScriptReference/KeyCode.Joystick8Button19.html">External documentation for `KeyCode.Joystick8Button19`</a></footer>
        Joystick8Button19 = 509, // 0x000001FD
        
        /// <summary>
        /// add IMU key code.
        /// </summary>
        L_Key_A = 510,
        R_Key_A = 511,
        L_Key_B = 512,
        R_Key_B = 513,
        L_Key_Menu = 514,
        R_Key_Menu = 515,
        L_JoyStick = 516,
        R_JoyStick = 517,
        L_Key_L1 = 518,
        R_Key_L1 = 519,
        L_Key_L2 = 520,
        R_Key_L2 = 521,
        
        L_JoyStick_H = 522,
        L_JoyStick_V = 523,
        R_JoyStick_H = 524,
        R_JoyStick_V = 525,
    }
}