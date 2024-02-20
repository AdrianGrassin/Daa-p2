using System.Collections.Generic;
using System.Linq;

namespace MaquinaRam
{
  public class RamController
  {
    private DataMemory dataMemory_;
    private ProgramMemory programMemory_;
    private InputTape inputTape_;
    private OutputTape outputTape_;


    public RamController(string program, string InputTape)
    {
      dataMemory_ = new DataMemory();
      programMemory_ = new ProgramMemory(program);
      inputTape_ = new InputTape(InputTape.Split(' ').Select(int.Parse).ToList());
      outputTape_ = new OutputTape();
    }

    public void Run()
    {
      while (programMemory_.GetProgramCounter() < programMemory_.GetInstructions().Count && programMemory_.GetProgramCounter()  >= 0)
      {
        var instruction = programMemory_.GetInstructions()[programMemory_.GetProgramCounter()];
        programMemory_.IncrementProgramCounter();
        instruction.Execute(ref dataMemory_, ref programMemory_, ref inputTape_, ref outputTape_);
      }
    }
     
    public string GetOutput()
    {
      return string.Join(" ", outputTape_.GetOutput());
    }

    public void Reset(string new_input, string new_program)
    {
      dataMemory_.Reset();
      programMemory_ = new ProgramMemory(new_program);
      outputTape_.Reset();
      inputTape_.Reset(new_input);
    }

    public Dictionary<int,int> GetRegisters()
    {
      return dataMemory_.GetRegisters();
    }

    public int GetAcc()
    {
      return dataMemory_.GetAcc();
    }

  }
}