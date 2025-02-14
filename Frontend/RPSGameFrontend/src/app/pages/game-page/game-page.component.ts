import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { GameService } from '../../services/game.service';
import { AuthService } from '../../services/auth-service.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PlayerMoves } from '../../cores/enums/playerMoves';
import {Message } from '../../cores/enums/Message';
import { GameStates } from '../../cores/enums/gameStates';
import { ActivatedRoute, Router } from '@angular/router';


@Component({
  selector: 'app-game-page',
  imports: [CommonModule, FormsModule],
  templateUrl: './game-page.component.html',
  styleUrls: ['./game-page.component.scss'],
})
export class GamePageComponent implements OnInit, OnDestroy {
  authService = inject(AuthService);
  gameService = inject(GameService);
  router = inject(Router)
  gameStates = GameStates;
  selectedChoice: PlayerMoves | null = null;
  roomState: GameStates = GameStates.WaitingPlayers;
  message: Message |null = null;
  playerMoves = PlayerMoves;
  timer: number = 10; 
  private userId: string = this.authService.getUserIdFromToken();
  roomId: string | null = null;
  private intervalId: any; 

  
  selectChoice(choice: PlayerMoves) {
    this.selectedChoice = choice;
  }
  changeStatus() {
    const lastMessage = this.message
    if (!lastMessage) return; 

    if (lastMessage.currentRoundWinnerID === null) {
        console.log('Ожидаем чужой ход');
    } 
    else 
    {
        if (lastMessage.currentRoundWinnerID === this.userId) 
          {
            this.roomState = GameStates.RoundWin;
            setTimeout(() => 
            {
                console.log("[A]Прошло 5 секунд после изменения статуса");
                if (lastMessage.gameWinnerId == null) 
                  {
                    this.roomState = GameStates.Round;
                    this.startRound();
                  } 
                else 
                {
                    this.roomState = GameStates.GameWin;
                    setTimeout(() => 
                      {
                        console.log("[B]Прошло 5 секунд после изменения статуса");
                        this.router.navigate(['']);
                        this.roomState = GameStates.WaitingPlayers
                        this.ngOnDestroy();
                      }, 5000);
                }
            }, 5000);
        } 
        else 
        {
            this.roomState = GameStates.RoundLoss;
            setTimeout(() => 
              {
                console.log("Прошло 5 секунд после изменения статуса");
                if (lastMessage.gameWinnerId == null) 
                  {

                    this.startRound();
                  } 
                else 
                {
                    this.roomState = GameStates.GameLoss;
                    setTimeout(() => 
                      {
                        console.log("Прошло 5 секунд после изменения статуса");
                        this.router.navigate(['']);
                      }, 5000);
                }
            }, 5000);
        }
    }
  }
  
  startRound() {
    this.roomState = GameStates.Round;
    
    setTimeout(() => 
    {
        if (this.selectedChoice !== null) 
        {
          console.log("norm")
          this.gameService.SendMove(this.selectedChoice);
          this.selectedChoice = null;
        }
        else
        {
          console.log("7")
          setTimeout(() => 
            {          
              this.gameService.SendMove(PlayerMoves.Paper);
              this.selectedChoice = null;
            }, 7000);
        }
      }, 5000);
  }

  ngOnInit() {
    this.roomState = GameStates.WaitingPlayers;
    this.userId = this.authService.getUserIdFromToken();
    this.gameService.startConnection(this.userId, this.roomId!);
      this.gameService.allPlayersInRoom.subscribe(() => {
        setTimeout(() => 
          {
            this.startRound();
          }, 5000);
      });
    this.gameService.message.subscribe(message => {
      this.message = message[0];
      this.changeStatus()
    });
  }
  constructor(private route: ActivatedRoute) {
    this.roomId = this.route.snapshot.paramMap.get('roomId');
    console.log('Room ID:', this.roomId);
  }

  resetState(): void {
    this.selectedChoice = null;
    this.roomState = GameStates.WaitingPlayers;
    this.message = null;
    this.timer = 10;
    this.roomId = null;
    if (this.intervalId) {
      clearInterval(this.intervalId);
      this.intervalId = null;
    }
    console.log("resetState");
  }



  ngOnDestroy() {
    clearInterval(this.intervalId); 
    this.resetState();
    this.gameService.OnDisconnectedAsync(null);
  }
}
