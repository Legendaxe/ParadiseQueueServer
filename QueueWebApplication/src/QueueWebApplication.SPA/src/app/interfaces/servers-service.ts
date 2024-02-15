import {Server} from "./server";

export interface IServersService {
  getAllServers(): Promise<Server[]>;
}
