using System;
using System.Collections.Generic;
using System.Linq;

namespace MaquinaRam
{
  public class ProgramMemory
  {
  private int _programCounter { get; set; } = 0;
  private Dictionary<string, int> _labels { get; set; }
  private List<Instruction> _instructions { get; set; }

  public ProgramMemory(string program) {
    _instructions = [];
    _programCounter = 0;
    _labels = [];

    if (program == null || program == "")
    {
      return;
    }
    var lines = program.Split(Environment.NewLine); 
    // remove all # comments
    lines = lines.Select(line => line.Split('#')[0]).ToArray();
    //remove all empty lines
    lines = lines.Where(line => !string.IsNullOrEmpty(line)).ToArray();
    

    for (int i = 0; i < lines.Length; i++)
    {
      var line = lines[i];

      // quitamos los comentarios
      line = line.Replace('\t', ' ');
      string[] parts = line.Split('#')[0].Trim().Split(' ');
      parts = parts.Where(s => !string.IsNullOrEmpty(s)).ToArray();

      if(parts.Length == 0 || parts[0] == "") continue;

      // convertimos a mayusculas para que no sea case sensitive
      parts = parts.Select(part => part.ToUpper()).ToArray();
      
      // label checker
      if(parts[0].EndsWith(':'))
      {
        var label = parts[0][..^1];
        _labels.Add(label, i);
        // si es un label tiene una instuccion en la misma linea así que hay que parsear la instruccion
        if (parts.Length == 1) continue;

        parts = parts[1..];        
      }
      // si no es un label, es una instruccion
      if (parts.Length != 2 && parts[0] != "HALT")
      {
        throw new Exception($"Invalid sintax at line {i+1}, expected 2 words, got {parts.Length}");
      }

      if (parts[0] == "HALT")
      {
        _instructions.Add(new Halt());
        continue;
      }

      string instruction = parts[0];
      instruction = instruction.ToUpper();
      string operand = parts[1];

      // Manejo del operando
      Operand op = new(operand, i);
      
      switch (instruction)
      {
        case "ADD":
          _instructions.Add(new Add(op));
          break;
        case "SUB":
          _instructions.Add(new Sub(op));
          break;
        case "MUL":
          _instructions.Add(new Mul(op));
          break;
        case "DIV":
          _instructions.Add(new Div(op));
          break;
        case "LOAD":
          _instructions.Add(new Load(op));
          break;
        case "STORE":
          _instructions.Add(new Store(op));
          break;
        case "JUMP":
          _instructions.Add(new Jump(op));
          break;
        case "JZERO":
          _instructions.Add(new JumpIfZero(op));
          break;
        case "JGTZ":
          _instructions.Add(new JumpIfGreaterThanZero(op));
          break;
        case "READ":
          _instructions.Add(new Read(op));
          break;
        case "WRITE":
          _instructions.Add(new Write(op));
          break;
        default:
          throw new Exception($"Invalid instruction {instruction} at line {i+1}");
      }
    }
  }

  public Instruction GetNextInstruction()
  {
    if (_programCounter >= _instructions.Count)
    {
      throw new Exception("No more instructions to execute");
    }
    return _instructions[_programCounter++];
  }

  public void IncrementProgramCounter()
  {
    _programCounter++;
  }

  public void SetProgramCounter(int value)
  {
    _programCounter = value;
  }

  public int getLabelAddress(string label)
  {
    if(!_labels.ContainsKey(label) || label == null)
    {
      throw new Exception($"Label {label} not found");
    }
    return _labels[label];
  }

  public int GetProgramCounter()
  {
    return _programCounter;
  }

  public List<Instruction> GetInstructions()
  {
    return _instructions;
  }
  }
}


