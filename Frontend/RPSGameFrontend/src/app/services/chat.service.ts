import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../enviroment/environment';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private hubConnection!: signalR.HubConnection;
  private messagesSubject = new BehaviorSubject<any[]>(this.getStoredMessages());
  public messages$ = this.messagesSubject.asObservable();

  constructor() {
    this.restoreConnection();
  }

  startConnection(userId: string) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.chatServiceUrl}/chatHub`)
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('Connected to ChatHub');
        localStorage.setItem('userId', userId);
        this.joinChat(userId);
        this.listenForMessages();
      })
      .catch(err => console.error('Error while starting SignalR connection: ', err));
  }

  joinChat(userId: string) {
    if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
      console.log("userId:", userId);
      this.hubConnection.invoke('JoinChatAsync', userId)
        .then(() => console.log(`User ${userId} joined the chat`))
        .catch(err => console.error('Error joining chat: ', err));
    }
  }

  sendMessage(message: string) {
    if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
      this.hubConnection.invoke('SendMessageAsync', message)
        .catch(err => console.error('Error sending message: ', err));
    }
  }

  private listenForMessages() {
    this.hubConnection.off('ReceiveMessage');
    this.hubConnection.on('ReceiveMessage', (message) => {
      console.log('New message received:', message);
      const updatedMessages = [...this.messagesSubject.value, message];
      this.messagesSubject.next(updatedMessages);
      this.storeMessages(updatedMessages);
    });
  }
  
  stopConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop()
        .then(() => {
          console.log('Disconnected from ChatHub');
          localStorage.removeItem('userId');
          this.messagesSubject.next([]);
        })
        .catch(err => console.error('Error while stopping SignalR connection: ', err));
    }
  }

  private restoreConnection() {
    const storedUserId = localStorage.getItem('userId');
    if (storedUserId) {
      console.log('Restoring connection...');
      this.startConnection(storedUserId);
    }
  }

  private storeMessages(messages: any[]) {
    localStorage.setItem('chatMessages', JSON.stringify(messages));
  }

  private getStoredMessages(): any[] {
    const storedMessages = localStorage.getItem('chatMessages');
    return storedMessages ? JSON.parse(storedMessages) : [];
  }
}