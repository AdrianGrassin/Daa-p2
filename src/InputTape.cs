using System;
using System.Collections.Generic;
using System.Linq;

namespace MaquinaRam
{
  public class InputTape
  {
    private List<int> _input;
    private int _index;
    public InputTape(List<int> input)
    {
      _input = input;
      _index = 0;
    }

    public int Read()
    {
      if (_index >= _input.Count)
      {
        return 0;
      }
      return _input[_index++];
    }

    internal void Reset(string new_input)
    {
        _input = new_input.Trim().Split(' ').Select(int.Parse).ToList();
        _index = 0;
    }
  }
}