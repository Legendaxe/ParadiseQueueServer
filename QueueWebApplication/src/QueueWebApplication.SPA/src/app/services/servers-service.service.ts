import { Injectable } from '@angular/core';
import {IServersService} from "../interfaces/servers-service";
import {Server} from "../interfaces/server";

@Injectable({
  providedIn: 'root'
})
export class ServersServiceService implements IServersService {

  constructor() { }

  async getAllServers(): Promise<Server[]> {
    return [
      {
        ipAddress: '123.321.323',
        currentPlayers: 500,
        maximumPlayers: 100
      },
      {
        ipAddress: '123.32341.323',
        currentPlayers: 200,
        maximumPlayers: 100
      },
      {
        ipAddress: '123123.321.323',
        currentPlayers: 300,
        maximumPlayers: 100
      },
    ];
  }

}
