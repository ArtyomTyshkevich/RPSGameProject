import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Subject } from 'rxjs';
import { PlayerMoves } from '../cores/enums/playerMoves';
import { Message } from '../cores/enums/Message';
import { environment } from '../enviroment/environment';

@Injectable({
  providedIn: 'root'
})
export class GameService {
  private hubConnection!: signalR.HubConnection;
  public message = new BehaviorSubject<Message[]>([]);
  public allPlayersInRoom = new Subject<void>();

  constructor() {}

  startConnection(userId: string, roomId: string) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.gameServiceUrl}/gameHub`)
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();
    this.hubConnection
      .start()
      .then(() => {
        console.log('Connected to GameHub');
        this.joinChat(userId, roomId);
        this.listenForMessages();
      })
      .catch(err => console.error('Error while starting SignalR connection: ', err));
  }

  joinChat(userId: string, roomId: string) {
    console.log(roomId)
    if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
      this.hubConnection.invoke('JoinChat', userId, roomId)
        .then(() => console.log(`User ${userId} joined the chat`))
        .catch(err => console.error('Error joining chat: ', err));
    }
  }

  SendMove(move: PlayerMoves) {
    if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
      this.hubConnection.invoke('SendMove', move)
        .catch(err => console.error('Error sending message: ', err));
    }
  }

  private listenForMessages() {
    this.hubConnection.on('ReceiveMessage', (message) => {
      console.log('New message received:', message);
      this.message.next([message]);
    });
  
    this.hubConnection.on('AllPlayersInRoom', () => {
      console.log('All players are in the room!');
      this.notifyAllPlayersInRoom();
    });
  }

  public notifyAllPlayersInRoom() {
    this.allPlayersInRoom.next();
  }
  
  OnDisconnectedAsync(error: Error | null) {
    if (this.hubConnection) {
      this.hubConnection.stop()
        .then(() => {
          this.message.next([]);
  
          if (error) {
            console.error('Disconnected with error: ', error);
          } else {
            console.log('Disconnected from GameHub');
          }
        })
        .catch(err => console.error('Error while stopping SignalR connection: ', err));
    }
  }
}