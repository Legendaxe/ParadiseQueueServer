import {Component, Input} from '@angular/core';
import {Server} from "../interfaces/server";

@Component({
  selector: 'app-server-card',
  standalone: true,
  imports: [],
  templateUrl: './server-card.component.html',
  styleUrl: './server-card.component.css'
})
export class ServerCardComponent {
  @Input({ required: true}) server!:Server;
}
