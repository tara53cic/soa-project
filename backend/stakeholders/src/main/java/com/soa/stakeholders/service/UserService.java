package com.soa.stakeholders.service;

import com.soa.stakeholders.dto.UserDTO;
import com.soa.stakeholders.dto.UserRegistrationDto;
import com.soa.stakeholders.model.User;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public interface UserService {
    User findById(Long id);
    User findByUsername(String username);
    List<User> findAll ();
    User save(UserRegistrationDto userRequest);
    User save(User user);
    User update(Long id, UserDTO userDto);
}
