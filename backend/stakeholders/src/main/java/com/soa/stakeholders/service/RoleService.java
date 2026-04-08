package com.soa.stakeholders.service;

import com.soa.stakeholders.model.Role;
import org.springframework.stereotype.Service;

@Service
public interface RoleService {
    Role findById(Long id);
    Role findByName(String name);
}
