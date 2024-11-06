import { Injectable } from '@angular/core';
import { Server } from '../interfaces/server';
import { environment } from '../../environments/environment';
import * as signalR from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { QueuePosition } from '../interfaces/queue-position';
import { ServerStatus } from '../interfaces/server-status';

@Injectable({
  providedIn: 'root',
})
export class ServersServiceService {
  private hubConnection?: signalR.HubConnection;
  private pendingServersSubject = new Subject<Server[]>();
  private pendingServersStatusSubject = new Subject<ServerStatus[]>();
  private pendingQueuePositionSubject = new Subject<QueuePosition>();

  serversUpdated$: Observable<Server[]> = this.pendingServersSubject.asObservable();
  serversStatusUpdated$: Observable<ServerStatus[]> = this.pendingServersStatusSubject.asObservable();
  queuePositionUpdated$: Observable<QueuePosition> = this.pendingQueuePositionSubject.asObservable();

  connect() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.serversApi, {
        withCredentials: sessionStorage.getItem('token') != null,
        accessTokenFactory: () => {
          let token = sessionStorage.getItem('token');
          return token ?? '';
        },
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
      })
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connected to SignalR hub'))
      .catch((err) => {
        console.error('Error connecting to SignalR hub:', err);
        throw err;
      });

    this.hubConnection.on('PendingServersInitData', (servers: Server[]) => {
      this.pendingServersSubject.next(servers);
    });
    this.hubConnection.on('PendingServersStatusData', (serversStatus: Server[]) => {
      this.pendingServersStatusSubject.next(serversStatus);
    });
    this.hubConnection.on('PendingQueuePosition', (queuePosition: QueuePosition) => {
      this.pendingQueuePositionSubject.next(queuePosition);
    });
    return false;
  }
}
