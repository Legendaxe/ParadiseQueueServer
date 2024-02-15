import { TestBed } from '@angular/core/testing';

import { ServersServiceService } from './servers-service.service';

describe('ServersServiceService', () => {
  let service: ServersServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServersServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
