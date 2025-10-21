namespace TetrisCSharp.Domain.Model;

public enum TetrominoType { I, O, T, S, Z, J, L }
public enum Rotation { R0, R90, R180, R270 }
public enum RotationDir { CW, CCW }
public enum CellState { Empty, Locked } // color/char se decide en UI
