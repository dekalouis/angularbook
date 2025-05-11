import { Injectable } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { Router } from '@angular/router';

export const authRedirectGuard: CanActivateFn = () => {
  const router = inject(Router);
  const token = localStorage.getItem('token');

  if (token) {
    router.navigate(['/books']);
    return false;
  }

  return true;
};
