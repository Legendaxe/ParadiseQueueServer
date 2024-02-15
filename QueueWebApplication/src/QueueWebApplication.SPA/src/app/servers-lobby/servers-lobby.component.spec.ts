import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ServersLobbyComponent } from './servers-lobby.component';

describe('ServersLobbyComponent', () => {
  let component: ServersLobbyComponent;
  let fixture: ComponentFixture<ServersLobbyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ServersLobbyComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ServersLobbyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
