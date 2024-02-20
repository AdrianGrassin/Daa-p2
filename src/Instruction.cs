using System;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

/// Debería hacer varias interfaces, Arithmetic, Jump, ReadWrite, Halt

namespace MaquinaRam
{

  public interface Instruction
  {
      void Execute(ref DataMemory dataMemory, ref ProgramMemory programMemory, ref InputTape inputTape, ref OutputTape outputTape);
  }  

  public class Add(Operand op) : Instruction
  {
    private readonly Operand op_ = op;

    public void Execute(ref DataMemory dataMemory, ref ProgramMemory programMemory, ref InputTape inputTape, ref OutputTape outputTape)
    {
      switch (op_.Type())
      {
        case OperandType.Inmediate:
          dataMemory.SetAcc(dataMemory.GetAcc() + op_.Value());
          break;
        
        case OperandType.Direct:
          dataMemory.SetAcc(dataMemory.GetDirectRegister(op_.Value()) + dataMemory.GetAcc());
          break;

        case OperandType.Indirect:
          dataMemory.SetAcc(dataMemory.GetDirectRegister(dataMemory.GetDirectRegister(op_.Value())) + dataMemory.GetAcc());
          break;
      }
    }
  }

  public class Sub(Operand op) : Instruction
  {
    private readonly Operand op_ = op;

    public void Execute(ref DataMemory dataMemory, ref ProgramMemory programMemory, ref InputTape inputTape, ref OutputTape outputTape)
    {
      switch (op_.Type())
      {
        case OperandType.Inmediate:
          dataMemory.SetAcc(dataMemory.GetAcc() - op_.Value());
          break;
        
        case OperandType.Direct:
          dataMemory.SetAcc(dataMemory.GetDirectRegister(op_.Value()) - dataMemory.GetAcc());
          break;

        case OperandType.Indirect:
          dataMemory.SetAcc(dataMemory.GetDirectRegister(dataMemory.GetDirectRegister(op_.Value())) - dataMemory.GetAcc());
          break;
      }
    }
  }

  public class Mul(Operand op) : Instruction
  {
    private readonly Operand op_ = op;

    public void Execute(ref DataMemory dataMemory, ref ProgramMemory programMemory, ref InputTape inputTape, ref OutputTape outputTape)
    {
      switch (op_.Type())
      {
        case OperandType.Inmediate:
          dataMemory.SetAcc(dataMemory.GetAcc() * op_.Value());
          break;
        
        case OperandType.Direct:
          dataMemory.SetAcc(dataMemory.GetDirectRegister(op_.Value()) * dataMemory.GetAcc());
          break;

        case OperandType.Indirect:
          dataMemory.SetAcc(dataMemory.GetDirectRegister(dataMemory.GetDirectRegister(op_.Value())) * dataMemory.GetAcc());
          break;
      }
    }
  }

  public class Div(Operand op) : Instruction
  {
    private readonly Operand _op = op;

    public void Execute(ref DataMemory dataMemory,  ref ProgramMemory programMemory, ref InputTape inputTape, ref OutputTape outputTape)
    {
      switch (_op.Type())
      {
        case OperandType.Inmediate:
          if(_op.Value() == 0) {
            throw new Exception("Cannot divide by zero");
          }
          dataMemory.SetAcc(dataMemory.GetAcc() / _op.Value());
          break;
        
        case OperandType.Direct:
          if(dataMemory.GetDirectRegister(_op.Value()) == 0) {
            throw new Exception("Cannot divide by zero");
          }
          dataMemory.SetAcc(dataMemory.GetDirectRegister(_op.Value()) / dataMemory.GetAcc());
          break;

        case OperandType.Indirect:
          if(dataMemory.GetDirectRegister(dataMemory.GetDirectRegister(_op.Value())) == 0) {
            throw new Exception("Cannot divide by zero");
          }
          dataMemory.SetAcc(dataMemory.GetDirectRegister(dataMemory.GetDirectRegister(_op.Value())) / dataMemory.GetAcc());
          break;
      }
    }
  }

  public class Load(Operand op) : Instruction
  {
    private readonly Operand op_ = op;
    public void Execute(ref DataMemory dataMemory,  ref ProgramMemory programMemory, ref InputTape inputTape, ref OutputTape outputTape)
    {
      switch (op_.Type())
      {
        case OperandType.Inmediate:
          dataMemory.SetAcc(op_.Value());
          break;
        
        case OperandType.Direct:
          dataMemory.SetAcc(dataMemory.GetDirectRegister(op_.Value()));
          break;

        case OperandType.Indirect:
          dataMemory.SetAcc(dataMemory.GetDirectRegister(dataMemory.GetDirectRegister(op_.Value())));
          break;
      }
    }
  }

  public class Store(Operand op) : Instruction
  {
    private readonly Operand op_ = op;
    public void Execute(ref DataMemory dataMemory,  ref ProgramMemory programMemory, ref InputTape inputTape, ref OutputTape outputTape)
    {
      switch (op_.Type())
      {
        case OperandType.Direct:
          dataMemory.SetDirectRegister(op_.Value(), dataMemory.GetAcc());
          break;

        case OperandType.Indirect:
          dataMemory.SetDirectRegister(dataMemory.GetDirectRegister(op_.Value()), dataMemory.GetAcc());
          break;
        default:
          throw new Exception("Invalid operand type, store only accepts direct and indirect operands.");
            
      }
    }
  }

  public class Jump : Instruction
  {
    private readonly Operand label_;
    public Jump(Operand op)
    {
      label_ = op;
    }
    public void Execute(ref DataMemory dataMemory,  ref ProgramMemory programMemory, ref InputTape inputTape, ref OutputTape outputTape)
    {
      if (label_.Label() != null){
        programMemory.SetProgramCounter(programMemory.getLabelAddress(label_.Label()));
      } 
      else 
      {
        throw new Exception($"The label is null");
      }
    }
  }

  public class JumpIfZero(Operand op) : Instruction
  {
    private readonly Operand label_ = op;
    public void Execute(ref DataMemory dataMemory,  ref ProgramMemory programMemory, ref InputTape inputTape, ref OutputTape outputTape)
    {
      if (dataMemory.GetAcc() == 0 && label_.Label() != null)
      {
        programMemory.SetProgramCounter(programMemory.getLabelAddress(label_.Label()));
      }
    }
  }

  public class Exp(Operand op) : Instruction
  {
    private readonly Operand op_ = op;

    static int IntPow(int x, int y)
    {
      int result = 1;
      for (int i = 0; i < y; i++)
      {
        result *= x;
      }
      return result;
    }

    public void Execute(ref DataMemory dataMemory, ref ProgramMemory programMemory, ref InputTape inputTape, ref OutputTape outputTape)
    {
      switch (op_.Type())
      {
        case OperandType.Inmediate:
          dataMemory.SetAcc(IntPow(dataMemory.GetAcc(), op_.Value()));
          break;
        
        case OperandType.Direct:
          dataMemory.SetAcc(IntPow(dataMemory.GetDirectRegister(op_.Value()), dataMemory.GetAcc()));
          break;

        case OperandType.Indirect:
          dataMemory.SetAcc(IntPow(dataMemory.GetDirectRegister(dataMemory.GetDirectRegister(op_.Value())), dataMemory.GetAcc()));
          break;
      }
    }
  }

  public class Read(Operand op) : Instruction
  {
    private readonly Operand op_ = op;
    public void Execute(ref DataMemory dataMemory, ref ProgramMemory programMemory, ref InputTape inputTape, ref OutputTape outputTape)
    {
      if(op_.Value() == 0) {
        throw new Exception("Cannot read into the accumulator");
      }
      dataMemory.SetDirectRegister(op_.Value(), inputTape.Read());
    }
  }

  public class Write(Operand op) : Instruction
  {
    private readonly Operand op_ = op;
    public void Execute(ref DataMemory dataMemory, ref ProgramMemory programMemory, ref InputTape inputTape, ref OutputTape outputTape)
    {
      switch(op_.Type())
      {
        case OperandType.Inmediate:
          outputTape.Write(op_.Value());
          break;
        case OperandType.Direct:
          if(op_.Value() == 0) {
            throw new Exception("Cannot write from the Register R0");
          }
          outputTape.Write(dataMemory.GetDirectRegister(op_.Value()));
          break;
        case OperandType.Indirect:
          if(dataMemory.GetDirectRegister(dataMemory.GetDirectRegister(op_.Value())) == 0) {
            throw new Exception("Cannot write to the Register R0");
          }
          outputTape.Write(dataMemory.GetDirectRegister(dataMemory.GetDirectRegister(op_.Value())));
          break;
      }
    }
  }

  public class Halt : Instruction
  {
    public void Execute(ref DataMemory dataMemory, ref ProgramMemory programMemory, ref InputTape inputTape, ref OutputTape outputTape)
    {
      programMemory.SetProgramCounter(-1);
    }
  }

  public class JumpIfGreaterThanZero(Operand operand) : Instruction
  {
    private readonly Operand operand_ = operand;
    public void Execute(ref DataMemory dataMemory, ref ProgramMemory programMemory, ref InputTape inputTape, ref OutputTape outputTape)
    {
      if(dataMemory.GetAcc() > 0)
      {
        programMemory.SetProgramCounter(programMemory.getLabelAddress(operand_.Label()));
      }
    }
  }
  
}

