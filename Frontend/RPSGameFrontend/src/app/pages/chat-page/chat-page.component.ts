import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { ChatService } from '../../services/chat.service';
import { AuthService } from '../../services/auth-service.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-chat',
  imports: [CommonModule, FormsModule],
  templateUrl: './chat-page.component.html',
  styleUrls: ['./chat-page.component.scss']
})
export class ChatPageComponent implements OnInit, OnDestroy {
  message = '';
  messages: any[] = [];
  authService = inject(AuthService);
  constructor(private chatService: ChatService) {}

  ngOnInit() {
    const userId = this.authService.getUserIdFromToken();
    console.log(userId)
    this.chatService.startConnection(userId);

    this.chatService.messages$.subscribe(messages => {
      this.messages = messages;
    });
  }

  sendMessage() {
    if (this.message.trim()) {
      this.chatService.sendMessage(this.message);
      this.message = '';
    }
  }

  ngOnDestroy() {
    this.chatService.stopConnection();
    this.messages = [];
  }
}
