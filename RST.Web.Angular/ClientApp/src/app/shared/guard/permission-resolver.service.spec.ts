import { TestBed, inject } from '@angular/core/testing';

import { PermissionResolverService } from './permission-resolver.service';

describe('PermissionResolverService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PermissionResolverService]
    });
  });

  it('should be created', inject([PermissionResolverService], (service: PermissionResolverService) => {
    expect(service).toBeTruthy();
  }));
});
