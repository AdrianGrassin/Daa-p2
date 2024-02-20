using System;
using System.Collections.Generic;

namespace MaquinaRam
{
  public class OutputTape
  {
    private List<int> _output;
    public OutputTape()
    {
      _output = new List<int>();
    }

    public void Write(int value)
    {
      _output.Add(value);
    }

    public List<int> GetOutput()
    {
      return _output;
    }

    internal void Reset()
    {
      _output = new List<int>();
    }
  }
}