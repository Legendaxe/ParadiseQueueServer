import { Component } from '@angular/core';
import { ServersLobbyComponent } from '../servers-lobby/servers-lobby.component';

@Component({
  selector: 'app-queue-page',
  standalone: true,
  imports: [ServersLobbyComponent],
  templateUrl: './queue-page.component.html',
  styleUrl: './queue-page.component.css',
})
export class QueuePageComponent {}
