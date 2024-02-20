using System;
using System.Collections.Generic;

namespace MaquinaRam
{
  public class DataMemory
  {
    private int acc; // r0
    private Dictionary<int, int> registers;  
    public DataMemory(){
      acc = 0;
      registers = [];
    }

    public int GetAcc() 
    {
      return acc;
    }

    public void SetAcc(int value) 
    {
      acc = value;
    }

    public int GetDirectRegister(int index)
    {
      if(!registers.ContainsKey(index))
      {
        throw new Exception("Se intentó acceder a un registro no existente");
      }
      return registers[index];
    }

    public void SetDirectRegister(int index, int value)
    {
      if(!registers.ContainsKey(index))
      {
        registers.Add(index, value);
      } else {
        registers[index] = value;
      }
    }

    internal void Reset()
    {
        acc = 0;
        registers = [];
    }

    public Dictionary<int, int> GetRegisters()
    {
      return registers;
    }
  }
}