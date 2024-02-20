using System;
using System.Text.RegularExpressions;

namespace MaquinaRam
{
    public enum OperandType
  {
    Inmediate,
    Direct,
    Indirect,
    Label,

    NotSupported

  }

  public class Operand
  {
    private int _value { get; set; }
    private string? _label { get; set; }
    private OperandType _type { get; set; }
    public Operand(string op, int i)
    {
      switch (op[0])
      {
        case '=':
          _type = OperandType.Inmediate;
          op = op[1..];
          break;
        case '*':
          _type = OperandType.Indirect;
          op = op[1..];
          break;
        case var _ when Regex.IsMatch(op, "[0-9]"):
          _type = OperandType.Direct;
          break;
        case var _ when Regex.IsMatch(op, "[A-Z]"):
          _type = OperandType.Label;
          _label = op;
          break;
        default:
          _type = OperandType.NotSupported;
          break;
      }

      if (_type == OperandType.NotSupported) 
      {
        throw new Exception($"Sintax error! operand {op} found in line {i} is not a valid operand!");
      }

      if (_type != OperandType.Label)
      {
        _value = int.Parse(op);
      }
    }

    public OperandType Type() {
      return _type;
    }

    public int Value() {
      return _value;
    }

    public string? Label() {
      return _label;
    }
  }
}