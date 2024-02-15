import {Routes} from '@angular/router';
import {AppComponent} from "./app.component";
import {ServersLobbyComponent} from "./servers-lobby/servers-lobby.component";

const routeConfig: Routes = [
  {
    path: "",
    component: ServersLobbyComponent,
    title: 'Servers'
  },
]

export default routeConfig;
