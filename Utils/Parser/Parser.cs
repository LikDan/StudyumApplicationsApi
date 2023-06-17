using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using TimeoutException = ApplicationsApi.Utils.Parser.Utils.TimeoutException;

namespace ApplicationsApi.Utils.Parser;

public partial class Parser {
    public int Timeout { get; set; }
    
    private HtmlDocument Html { get; }
    private ParserContext Context { get; }

    public Parser(string html, string input, int timeout = -1) : this(html, new ParserContext(input), timeout) { }

    public Parser(string html, object input, int timeout = -1) : this(html, new ParserContext(input), timeout) { }

    public Parser(string html, ParserContext context, int timeout = -1) {
        Html = new HtmlDocument();
        Html.LoadHtml(html);
        Timeout = timeout;

        Context = context;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public void Parse() {
        var task = Task.Run(() => {
            ParseLoops();
            ParseConditions();
            ParseVariables();
            ParseExpressions();
        });

        if (!task.Wait(TimeSpan.FromMilliseconds(Timeout)) || task.Status == TaskStatus.Running)
            throw new TimeoutException(Timeout);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public void ParseExpressions() {
        RecursiveEach<HtmlTextNode>(Html.DocumentNode.ChildNodes, n => {
            var regex = ExpressionRegex();
            var res = regex.Replace(n.InnerHtml, match => Execute(match.Value[2..^2]));

            n.InnerHtml = res;
        });
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public void ParseVariables() {
        RecursiveEach<HtmlTextNode>(Html.DocumentNode.ChildNodes, n => {
            var regex = VariableRegex();
            var res = regex.Replace(n.InnerHtml, match => Context.GetStringByName(match.Value[2..^2]).ToString());

            n.InnerHtml = res;
        });
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public void ParseConditions() {
        RecursiveEach(Html.DocumentNode.ChildNodes, IfTag, n => {
            var rootCondition = ParseCondition(n);

            HtmlNode? resNode = null;
            if (rootCondition) {
                for (var i = 0; i < n.ChildNodes.Count; i++) {
                    if (n.ChildNodes[i].Name != ElseTag && n.ChildNodes[i].Name != ElseIfTag) continue;
                    n.ChildNodes.RemoveAt(i);
                    i--;
                }

                resNode = n;
            }

            resNode ??=
                n.ChildNodes.FirstOrDefault(elseNode => elseNode.Name == ElseIfTag && ParseCondition(elseNode)) ??
                n.ChildNodes.FirstOrDefault(elseNode => elseNode.Name == ElseTag);

            if (resNode != null)
                foreach (var node in resNode.ChildNodes.Reverse()) {
                    n.ParentNode.InsertAfter(node, n);
                }

            n.Remove();
        });
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public void ParseLoops() {
        RecursiveEach(Html.DocumentNode.ChildNodes, LoopTag, n => {
            var arrayName = n.GetAttributeValue(LoopArrayAttr, null);
            if (arrayName == null || arrayName.Length < 3) return;
            arrayName = arrayName.Substring(1, arrayName.Length - 2);

            var array = Context.GetByName(arrayName);

            var indexVarName = n.GetAttributeValue(LoopIndexAttr, null);
            var valueVarName = n.GetAttributeValue(LoopValueAttr, null);

            var children = n.ChildNodes.ToList();
            n.RemoveAllChildren();

            for (var i = ParserContext.GetArrayLength(array) - 1; i >= 0; i--) {
                var replacementIndex = indexVarName == null
                    ? null
                    : new ReplacementValue($"{{{indexVarName}}}", i.ToString());
                var replacementValue =
                    valueVarName == null
                        ? null
                        : new ReplacementValue($"{{{valueVarName}", $"{{{arrayName}[{i.ToString()}]");

                foreach (var node in children.Select(t => t.Clone()).Reverse()) {
                    ParseLoop(node, replacementIndex, replacementValue);
                    n.ParentNode.InsertAfter(node, n);
                }
            }

            n.ParentNode.RemoveChild(n);
        });
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public void Trim() {
        RecursiveEach(Html.DocumentNode.ChildNodes, node => {
            if (node.InnerHtml.Trim() == "" && node is HtmlTextNode) node.Remove();
        });
    }

    public override string ToString() {
        return string.Join("", Html.DocumentNode.InnerHtml.Split("\n").Select(line => line.Trim()));
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private bool ParseCondition(HtmlNode node) {
        var condition = node.GetAttributeValue("condition", null);
        if (condition == null) return false;

        var regex = VariableRegex();
        var res = regex.Replace(condition, match => Context.GetStringByName(match.Value[1..^1]).ToString());

        return CSharpScript.EvaluateAsync<bool>(res).Result;
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private void ParseLoop(HtmlNode node, ReplacementValue? index, ReplacementValue? value) {
        foreach (var attribute in node.Attributes) {
            attribute.Value = ReplacementValue.Replace(attribute.Value, index, value);
        }

        if (node.ChildNodes.Count == 0) node.InnerHtml = ReplacementValue.Replace(node.InnerHtml, index, value);

        RecursiveEach(node.ChildNodes, n => {
            foreach (var attribute in n.Attributes) {
                attribute.Value = ReplacementValue.Replace(attribute.Value, index, value);
            }

            if (n.ChildNodes.Count != 0) return;

            n.InnerHtml = ReplacementValue.Replace(n.InnerHtml, index, value);
        });
    }

    private static void RecursiveEach<T>(IList<HtmlNode> nodes, Action<HtmlNode> onEach, string? nodeName = null)
        where T : HtmlNode {
        var i = 0;
        while (i < nodes.Count) {
            if ((nodeName == null || nodeName == nodes[i].Name) && nodes[i] is T) onEach(nodes[i]);
            if (i >= nodes.Count) return;
            RecursiveEach<T>(nodes[i].ChildNodes, onEach, nodeName);
            i++;
        }
    }

    private static void RecursiveEach(IList<HtmlNode> nodes, Action<HtmlNode> onEach, string? nodeName = null) {
        RecursiveEach<HtmlNode>(nodes, onEach, nodeName);
    }

    // ReSharper disable once UnusedMember.Local
    private static void RecursiveEach<T>(IList<HtmlNode> nodes, string nodeName, Action<HtmlNode> onEach)
        where T : HtmlNode {
        RecursiveEach<T>(nodes, onEach, nodeName);
    }

    private static void RecursiveEach(IList<HtmlNode> nodes, string nodeName, Action<HtmlNode> onEach) {
        RecursiveEach<HtmlNode>(nodes, onEach, nodeName);
    }

    private static string Execute(string code) {
        if (code == "null") return "";
        try {
            return CSharpScript.EvaluateAsync<object>(code).Result.ToString() ?? "";
        }
        catch (Exception) {
            return "";
        }
    }


    private const string LoopTag = "loop";
    private const string IfTag = "if";
    private const string ElseIfTag = "else-if";
    private const string ElseTag = "else";

    private const string LoopArrayAttr = "array";
    private const string LoopIndexAttr = "index";
    private const string LoopValueAttr = "value";

    public static string InstantParse(string html, string input, int timeout = -1) {
        var parser = new Parser(html, input, timeout);
        parser.Parse();
        parser.Trim();
        return parser.ToString();
    }

    public static string InstantParse(string html, object input, int timeout = -1) {
        var parser = new Parser(html, input, timeout);
        parser.Parse();
        parser.Trim();
        return parser.ToString();
    }

    public static string InstantParse(string html, ParserContext context, int timeout = -1) {
        var parser = new Parser(html, context, timeout);
        parser.Parse();
        parser.Trim();
        return parser.ToString();
    }

    private class ReplacementValue {
        // ReSharper disable once MemberCanBePrivate.Local
        public string Key { get; }

        // ReSharper disable once MemberCanBePrivate.Local
        public string Value { get; }

        public ReplacementValue(string key, string value) {
            Key = key;
            Value = value;
        }

        public static string Replace(string value, params ReplacementValue?[] replacements) {
            return replacements
                .Where(r => r != null)
                .Aggregate(value, (current, r) => current.Replace($"{r!.Key}", $"{r.Value}"));
        }
    }

    [GeneratedRegex("\\[\\[(.*?)]]")]
    private static partial Regex ExpressionRegex();

    [GeneratedRegex("\\{\\{(.*?)}}")]
    private static partial Regex VariableRegex();
}