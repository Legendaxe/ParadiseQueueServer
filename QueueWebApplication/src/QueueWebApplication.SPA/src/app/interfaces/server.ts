export interface Server {
  name: string;
  ipAddress: string;
  port: number;
  currentPlayers: number;
  maximumPlayers: number;
  queuePosition: number;
  whitelisted: boolean;
}
