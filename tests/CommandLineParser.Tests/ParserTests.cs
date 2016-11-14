﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CommandLineParser.Tests
{
    public class ParserTests
    {
        class Arguments
        {
            [Option('i', "integer")]
            public int Test { get; set; }

            [Option('t', "test")]
            public int AnotherTest { get; set; }

            [Value(0, "min")]
            public int Min { get; set; }
        }

        class ArgumentsWithLists
        {
            [Option('n', "numbers")]
            public IEnumerable<int> Numbers { get; set; }
        }

        [Fact]
        public void ParserShouldParseShortOptionToTestProperty()
        {
            var args = new[] {"-i", "4"};
            var parser = new Parser();
            parser.Register<Arguments>()
                .On<Arguments>(arguments => Assert.Equal(arguments.Test, int.Parse(args[1])))
                .Parse(args);
        }

        [Fact]
        public void ParserShouldParseLongOptionToTestProperty()
        {
            var args = new[] { "--integer", "4" };
            var parser = new Parser();
            parser.Register<Arguments>()
                .On<Arguments>(arguments => Assert.Equal(arguments.Test, int.Parse(args[1])))
                .Parse(args);
        }

        [Fact]
        public void ParserShouldParserMultipleProperties()
        {
            var args = new[] { "4", "-i", "4", "-t", "10" };
            var parser = new Parser();
            parser.Register<Arguments>()
                .On<Arguments>(arguments =>
                {
                    Assert.Equal(arguments.Test, 4);
                    Assert.Equal(arguments.AnotherTest, 10);
                    Assert.Equal(arguments.Min, 4);
                })
                .Parse(args);
        }

        [Fact]
        public void ParserShouldParseListsOfValuesForOption()
        {
            var args = new[] { "-n", "1", "2", "3", "4" };
            var parser = new Parser();
            parser.Register<ArgumentsWithLists>()
                .On<ArgumentsWithLists>(arguments =>
                {
                    Assert.Equal(arguments.Numbers, Enumerable.Range(1, 4));
                })
                .Parse(args);
        }
    }
}
