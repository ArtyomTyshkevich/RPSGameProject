import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { GameService } from '../../services/game.service';
import { AuthService } from '../../services/auth-service.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PlayerMoves } from '../../cores/enums/playerMoves';
import { Message } from '../../cores/enums/Message';
import { GameStates } from '../../cores/enums/gameStates';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, takeUntil, take } from 'rxjs';

@Component({
  selector: 'app-game-page',
  imports: [CommonModule, FormsModule],
  templateUrl: './game-page.component.html',
  styleUrls: ['./game-page.component.scss'],
})
export class GamePageComponent implements OnInit, OnDestroy {
  authService = inject(AuthService);
  gameService = inject(GameService);
  router = inject(Router);
  gameStates = GameStates;
  selectedChoice: PlayerMoves | null = null;
  roomState: GameStates = GameStates.WaitingPlayers;
  message: Message | null = null;
  playerMoves = PlayerMoves;
  timer: number = 10;
  private userId: string = this.authService.getUserIdFromToken();
  roomId: string | null = null;
  private intervalId: any;
  private destroy$ = new Subject<void>(); 

  constructor(private route: ActivatedRoute) {}

  selectChoice(choice: PlayerMoves) {
    this.selectedChoice = choice;
  }

  changeStatus() {
    const lastMessage = this.message;
    if (!lastMessage) return;

    if (lastMessage.firstPlayerMoves === null && lastMessage.secondPlayerMoves === null && lastMessage.gameWinnerId == null) {
      this.roomState = GameStates.Draw;
      setTimeout(() => {
        this.startRound();
      }, 5000);
      return;
    }

    if (lastMessage.currentRoundWinnerID === null) {
      return;
    }

    if (lastMessage.currentRoundWinnerID === this.userId) {
      this.handleRoundWin(lastMessage);
    } else {
      this.handleRoundLoss(lastMessage);
    }
  }

  handleRoundWin(lastMessage: Message) {
    this.roomState = GameStates.RoundWin;
    setTimeout(() => {
      if (lastMessage.gameWinnerId == null) {
        this.roomState = GameStates.Round;
        this.startRound();
      } else {
        this.roomState = GameStates.GameWin;
        setTimeout(() => {
          this.resetState();
          this.gameService.OnDisconnectedAsync(null);
          this.router.navigate(['']);
        }, 5000);
      }
    }, 5000);
  }

  handleRoundLoss(lastMessage: Message) {
    this.roomState = GameStates.RoundLoss;
    setTimeout(() => {
      if (lastMessage.gameWinnerId == null) {
        this.startRound();
      } else {
        this.roomState = GameStates.GameLoss;
        setTimeout(() => {
          this.resetState();
          this.gameService.OnDisconnectedAsync(null);
          this.router.navigate(['']);
        }, 5000);
      }
    }, 5000);
  }

  startRound() {
    this.roomState = GameStates.Round;

    setTimeout(() => {
      if (this.selectedChoice !== null) {
        this.gameService.SendMove(this.selectedChoice);
        this.selectedChoice = null;
      } else {
        this.gameService.SendMove(PlayerMoves.Paper);
        this.selectedChoice = null;
      }
    }, 5000);
  }

  ngOnInit() {
    this.roomState = GameStates.WaitingPlayers;
    this.userId = this.authService.getUserIdFromToken();
    this.roomId = this.route.snapshot.paramMap.get('roomId');

    this.gameService.startConnection(this.userId, this.roomId!);

    this.gameService.allPlayersInRoom.pipe(take(1)).subscribe(() => {
      setTimeout(() => {
        this.startRound();
      }, 5000);
    });

    this.gameService.message.pipe(takeUntil(this.destroy$)).subscribe((message) => {
      this.message = message[0];
      this.changeStatus();
    });
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
    console.log('resetState');
  }

  ngOnDestroy() {
    this.resetState();
    this.destroy$.next();
    this.destroy$.complete();
    this.gameService.OnDisconnectedAsync(null);
  }
}
