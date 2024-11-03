export interface PlayerJwt {
  sub: string;
  role: string;
  donTier: number;
  ban: boolean;
  wl: number[];
  nbf: number;
  exp: number;
  iat: number;
  iss: string;
  aud: string;
}
