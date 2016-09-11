<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bengali Machine translation</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellpadding="0" cellspacing="0" width="100%" border="0">
            <tr>
                <td valign="top">
                    <div>
                        <asp:TextBox ID="txtInput" runat="server" TextMode="MultiLine" Width="99.4%" Height="100"></asp:TextBox><br />
                        <asp:Button ID="Button1" runat="server" Text="Translate" OnClick="Button1_Click" />
                        <asp:Button ID="Button3" runat="server" Text="Test" OnClick="Button3_Click" />
                        <asp:Button ID="Button2" runat="server" Text="Analysis" OnClick="Button2_Click" /><br />
                        <b><asp:Literal ID="litBengali" runat="server"></asp:Literal></b>
                        <br />
                        <div style="overflow: auto; height: 300px;border:1px solid #CCC;">
                            <asp:Literal ID="litOutput" runat="server" ></asp:Literal>
                            <asp:TreeView ID="treeAnalysis" runat="server">
                            </asp:TreeView>
                        </div>
                    </div>
                </td>
                <td valign="top" style="width: 300px;">
                    <div style="overflow: auto; height: 500px;border:1px solid #CCC;">
                        <strong>CC : </strong> Coordinating conjunction
                        <br />
                        <strong>CD : </strong> Cardinal number
                        <br />
                        <strong>DT : </strong> Determiner
                        <br />
                        <strong>EX : </strong> Existential there
                        <br />
                        <strong>FW : </strong> Foreign word
                        <br />
                        <strong>IN : </strong> Preposition/subord. conjunction
                        <br />
                        <strong>JJ : </strong> Adjective
                        <br />
                        <strong>JJR : </strong> Adjective, comparative
                        <br />
                        <strong>JJS : </strong> Adjective, superlative
                        <br />
                        <strong>LS : </strong> List item marker
                        <br />
                        <strong>MD : </strong> Modal
                        <br />
                        <strong>NN : </strong> Noun, singular or mass
                        <br />
                        <strong>NNS : </strong> Noun, plural
                        <br />
                        <strong>NNP : </strong> Proper noun, singular
                        <br />
                        <strong>NNPS : </strong> Proper noun, plural
                        <br />
                        <strong>PDT : </strong> Predeterminer
                        <br />
                        <strong>POS : </strong> Possessive ending
                        <br />
                        <strong>PRP : </strong> Personal pronoun
                        <br />
                        <strong>PRP$ : </strong> Possessive pronoun
                        <br />
                        <strong>RB : </strong> Adverb
                        <br />
                        <strong>RBR : </strong> Adverb, comparative
                        <br />
                        <strong>RBS : </strong> Adverb, superlative
                        <br />
                        <strong>RP : </strong> Particle
                        <br />
                        <strong>SYM : </strong> Symbol (mathematical or scientific)
                        <br />
                        <strong>TO : </strong> to
                        <br />
                        <strong>UH : </strong> Interjection
                        <br />
                        <strong>VB : </strong> Verb, base form
                        <br />
                        <strong>VBD : </strong> Verb, past tense
                        <br />
                        <strong>VBG : </strong> Verb, gerund/present participle
                        <br />
                        <strong>VBN : </strong> Verb, past participle
                        <br />
                        <strong>VBP : </strong> Verb, non-3rd ps. sing. present
                        <br />
                        <strong>VBZ : </strong> Verb, 3rd ps. sing. present
                        <br />
                        <strong>WDT : </strong> wh-determiner
                        <br />
                        <strong>WP : </strong> wh-pronoun
                        <br />
                        <strong>WP$ : </strong> Possessive wh-pronoun
                        <br />
                        <strong>WRB : </strong> wh-adverb
                        <br />
                        <strong># : </strong> Pound sign
                        <br />
                        <strong>$ : </strong> Dollar sign
                        <br />
                        <strong>. : </strong> Sentence-final punctuation
                        <br />
                        <strong>, : </strong> Comma
                        <br />
                        <strong>: : </strong> Colon, semi-colon
                        <br />
                        <strong>( : </strong> Left bracket character
                        <br />
                        <strong>) : </strong> Right bracket character
                        <br />
                        <strong>" : </strong> Straight double quote
                        <br />
                        <strong>` : </strong> Left open single quote
                        <br />
                        <strong>" : </strong> Left open double quote
                        <br />
                        <strong>' : </strong> Right close single quote
                        <br />
                        <strong>" : </strong> Right close double quote
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
