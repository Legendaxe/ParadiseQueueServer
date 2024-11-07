
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { Server } from '../interfaces/server';
import { ServerStatus } from '../interfaces/server-status';
import { QueuePosition } from '../interfaces/queue-position';

@Injectable({
  providedIn: 'root',
})
export class MockServersServiceService {
  private pendingServersSubject = new Subject<Server[]>();
  private pendingServersStatusSubject = new Subject<ServerStatus[]>();
  private pendingQueuePositionSubject = new Subject<QueuePosition>();

  serversUpdated$: Observable<Server[]> = this.pendingServersSubject.asObservable();
  serversStatusUpdated$: Observable<ServerStatus[]> = this.pendingServersStatusSubject.asObservable();
  queuePositionUpdated$: Observable<QueuePosition> = this.pendingQueuePositionSubject.asObservable();
  connect() {
    let servers: Server[];
    servers = [
      {
        name: 'Green',
        maximumPlayers: -1,
        whitelisted: false,
        port: 4002,
        ipAddress: 's1.ss220.club',
        currentPlayers: 20,
        queuePosition: 20,
      },
      {
        name: 'Black',
        maximumPlayers: 100,
        whitelisted: false,
        port: 4000,
        ipAddress: 's1.ss220.club',
        currentPlayers: 100,
        queuePosition: 20,
      },
      {
        name: 'Prime',
        maximumPlayers: -1,
        whitelisted: true,
        port: 4001,
        ipAddress: 's1.ss220.club',
        currentPlayers: 20,
        queuePosition: 20,
      },
      {
        name: 'TG',
        maximumPlayers: -1,
        whitelisted: true,
        port: 4002,
        ipAddress: 's1.ss220.club',
        currentPlayers: 20,
        queuePosition: 20,
      },
    ];
    this.pendingServersSubject.next(servers);
  }
}
