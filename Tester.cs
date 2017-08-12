using System;
using System.Collections.Generic;

namespace effects_as_data_dotnet
{
    public static class Tester
    {
        public static Func<IEnumerable<T>> TestFunction<T>(Func<IEnumerable<T>> function, Func<IEnumerable<T>> spec)
        {
            return () =>
            {
                var actual = function().GetEnumerator();
                var expected = spec();

                foreach (var value in expected)
                {
                    var test = actual.MoveNext();
                    var test2 = actual.Current;
                }

                return TestRunner(function, expected);
            };
        }

        public static IEnumerable<T> TestRunner<T>(Func<IEnumerable<T>> function, IEnumerable<T> expected, int index = 0, Object previous = null)
        {
            // Add sanity check when function is null
            expected.GetEnumerator().MoveNext();

            return null;
        }
    }

    public class Args : IArg, IEnumerable<IArg>
    {
        public YieldCmd YieldCmd(ICommand command)
        {
            return (YieldCmd) Add(new YieldCmd(command));
        }

        public Returns Returns(Object obj)
        {
            return (Returns) Add(new Returns(obj));
        }

        public IEnumerator<IArg> GetEnumerator()
        {
            return new ArgEnumerator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class ArgEnumerator : IEnumerator<IArg>
    {
        IArg head;
        IArg current;
        public ArgEnumerator(IArg head)
        {
            this.head = head;
            this.current = head;
        }

        public IArg Current
        {
            get { return current; }
        }
 
        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        public void Reset()
        {
            this.current = this.head;
        }

        public bool MoveNext()
        {
            var next = this.current.next;
            if (next == null)
            {
                return false;
            }
            else
            {
                this.current = next;
                return true;
            }
        }

        void IDisposable.Dispose() { }
    }

    public class YieldCmd : IArg
    {
        public ICommand command { get; }
        public YieldCmd(ICommand command)
        {
            this.command = command;
        }

        public YieldReturns YieldReturns(Object obj)
        {
            return (YieldReturns) Add(new YieldReturns(obj));
        }
        public YieldReturnThrows YieldReturnThrows(Exception e)
        {
            return (YieldReturnThrows) Add(new YieldReturnThrows(e));
        }
        public ReturnCmdResult ReturnCmdResult()
        {
            // return (ReturnCmdResult) Add(new ReturnCmdResult(obj));
            return null;
        }
    }

    public class YieldReturns : IArg
    {
        public Object obj { get; }
        public YieldReturns(Object obj)
        {
            this.obj = obj;
        }
        public YieldCmd YieldCmd()
        {
            return null;
        }
        public ThrowAfterCmdReturns ThrowAfterCmdReturns()
        {
            return null;
        }
        public Returns Returns()
        {
            return null;
        }
    }

    public class ReturnCmdResult : IArg
    {

    }

    public class YieldReturnThrows : IArg
    {
        public Exception e { get; }
        public YieldReturnThrows(Exception e)
        {
            this.e = e;
        }
    }

    public class ThrowAfterCmdReturns : IArg
    {

    }

    public class Returns : IArg
    {
        public Object obj { get; }
        public Returns(Object obj)
        {
            this.obj = obj;
        }
    }

    public abstract class IArg
    {
        public IArg next { get; private set; }
        IList<IArg> args = new List<IArg>();
        protected T Add<T>(T arg) where T : IArg
        {
            next = arg;
            args.Add(arg);
            return arg;
        }
    }
}

public class Test
{
    public Object Input { get; }
    public Object ExpectedOutput { get; }
}
