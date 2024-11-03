import {Component, computed, input, OnInit} from '@angular/core';
import {Server} from "../interfaces/server";
import {QueueState} from "../enums/queue-state";
import {HttpClient} from "@angular/common/http";
import {takeUntilDestroyed, toObservable} from "@angular/core/rxjs-interop";
import {AsyncPipe, NgClass} from "@angular/common";
import {PlayerData} from "../interfaces/player-data";
import {firstValueFrom} from "rxjs";
import {QueueResponse} from "../enums/queue-response";
import {NumberOrInfinityPipe} from "../pipes/number-or-infinity.pipe";

@Component({
  selector: 'app-server-card',
  standalone: true,
  imports: [
    AsyncPipe,
    NgClass,
    NumberOrInfinityPipe
  ],
  templateUrl: './server-card.component.html',
  styleUrl: './server-card.component.css'
})

export class ServerCardComponent implements OnInit {
  server = input.required<Server>();
  playerData = input.required<PlayerData>();
  queueState = QueueState.NotInQueue;
  queuePosition$ = toObservable(computed(() => this.server().queuePosition + 1));
  disabled$ = toObservable(computed(() => {
    const playerData = this.playerData();
    const server = this.server();
    const notWhitelisted = server.whitelisted && !playerData.whitelistPasses.includes(server.port);
    return playerData.banned || notWhitelisted;
  }));
  serverUrl : string ='Error';
  protected readonly QueueState = QueueState;
  constructor(private http : HttpClient) {
    this.queuePosition$
      .pipe(takeUntilDestroyed())
      .subscribe(position => {
      if (position == 0) {
        this.queueState = QueueState.AllowedToConnect;
        this.playHornSound()
      }
    })
  }

  ngOnInit() {
    this.serverUrl = `byond://${this.server().ipAddress}:${this.server().port}`
    if (this.server().maximumPlayers == -1) {
      this.queueState = QueueState.AllowedToConnect;
    }
  }

  async connectToQueue() {
    const response = await firstValueFrom(this.http.post<QueueResponse>('/queue/add-client?serverName=' + this.server().name, {}));
    switch (response) {
      case QueueResponse.AddedToQueue:
        this.queueState = QueueState.InQueue;
        break;
      case QueueResponse.BypassedQueue:
        this.queueState = QueueState.AllowedToConnect;
        this.playHornSound();
        break;
      case QueueResponse.Rejected:
        throw new Error("You shouldn't be able to press connect button");
    }
  }

  private playHornSound() {
    const sound = new Audio('../assets/sounds/adminhelp.ogg');
    sound.load();
    sound.play().then();
  }
}
