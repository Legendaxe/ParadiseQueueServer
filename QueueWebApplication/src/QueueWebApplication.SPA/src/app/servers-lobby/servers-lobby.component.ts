import {Component, OnInit} from '@angular/core';
import {NgFor} from "@angular/common";
import {ServersServiceService} from "../services/servers-service.service";
import {Server} from "../interfaces/server";
import {ServerCardComponent} from "../server-card/server-card.component";


@Component({
  selector: 'app-servers-lobby',
  standalone: true,
  imports: [
    ServerCardComponent,
    NgFor
  ],
  templateUrl: './servers-lobby.component.html',
  styleUrl: './servers-lobby.component.css'
})
export class ServersLobbyComponent implements OnInit {
  constructor(private serversService: ServersServiceService) {}
  ngOnInit() {
    this.serversService.getAllServers().then((servers) => {
      this.servers = servers;
    });
  }

  servers: Server[] = [];
}
