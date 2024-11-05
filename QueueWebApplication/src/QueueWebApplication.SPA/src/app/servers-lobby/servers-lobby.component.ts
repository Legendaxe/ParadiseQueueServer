import { Component, signal } from '@angular/core';
import { NgFor } from '@angular/common';
import { ServersServiceService } from '../services/servers-service.service';
import { Server } from '../interfaces/server';
import { ServerCardComponent } from '../server-card/server-card.component';
import { map, take, toArray, zip } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PlayerData } from '../interfaces/player-data';
import { PlayerJwt } from '../interfaces/player-jwt';
import { MockServersServiceService } from '../services/mock-servers-service.service';

@Component({
  selector: 'app-servers-lobby',
  standalone: true,
  imports: [ServerCardComponent, NgFor],
  templateUrl: './servers-lobby.component.html',
  styleUrl: './servers-lobby.component.scss',
})
export class ServersLobbyComponent {
  servers = signal<Array<Server>>([]);
  haveToken = false;
  playerData: PlayerData = { ckey: '', role: '', donatorTier: 0, banned: false, whitelistPasses: [] };
  constructor(private serversService: MockServersServiceService) {
    let token = window.location.hash.split('#token=').pop() ?? '';
    if (!token) {
      return;
    }
    const jsonPayload = JSON.parse(atob(token.split('.')[1]));
    this.playerData = this.parsePlayerData(jsonPayload);

    sessionStorage.setItem('token', token);
    this.haveToken = true;
    this.serversService.serversUpdated$.pipe(take(1)).subscribe((servers) => {
      this.servers.set(
        servers.map((server) => {
          server.queuePosition = 0;
          return server;
        }),
      );
    });

    this.serversService.serversStatusUpdated$.pipe(takeUntilDestroyed()).subscribe((serversStatus) => {
      zip(this.servers(), serversStatus)
        .pipe(
          map(([server, serverStatus]) => {
            server.currentPlayers = serverStatus.currentPlayers;
            return server;
          }),
          toArray(),
        )
        .subscribe((updatedServers) => {
          this.servers.set(updatedServers);
        });
    });
    this.serversService.queuePositionUpdated$.pipe(takeUntilDestroyed()).subscribe((queuePosition) => {
      this.servers.update((servers) => {
        return servers.map((server) => {
          if (server.name == queuePosition.serverName) {
            return { ...server, queuePosition: queuePosition.position };
          }
          return server;
        });
      });
    });
    this.serversService.connect();
  }

  private parsePlayerData(data: PlayerJwt): PlayerData {
    return {
      ckey: data.sub,
      role: data.role,
      donatorTier: data.donTier,
      banned: data.ban,
      whitelistPasses: data.wl,
    };
  }
}
