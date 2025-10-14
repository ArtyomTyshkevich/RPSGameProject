import { PlayerMoves } from "./playerMoves";

export interface Message 
{
  firstPlayerMoves: PlayerMoves | null;
  secondPlayerMoves: PlayerMoves | null;
  currentRoundNum: number;
  currentRoundWinnerID: string | null; 
  gameWinnerId: string | null;
}