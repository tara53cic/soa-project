package com.soa.stakeholders.service.impl;

import com.soa.stakeholders.dto.UserDTO;
import com.soa.stakeholders.dto.UserRegistrationDto;
import com.soa.stakeholders.model.Role;
import com.soa.stakeholders.model.User;
import com.soa.stakeholders.repository.UserRepository;
import com.soa.stakeholders.service.RoleService;
import com.soa.stakeholders.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class UserServiceImpl implements UserService, UserDetailsService {

    @Autowired
    private UserRepository userRepository;

    @Autowired
    private PasswordEncoder passwordEncoder;

    @Autowired
    private RoleService roleService;

    @Autowired
    private Neo4jUserService neo4jUserService;

    @Override
    public UserDetails loadUserByUsername(String username) {
        User user = userRepository.findByUsername(username);
        if (user == null) {
            throw new RuntimeException (String.format("No user found with username '%s'.", username));
        } else {
            return user;
        }
    }

    @Override
    public User findByUsername(String username) {
        return userRepository.findByUsername(username);
    }

    public User findById(Long id)  {
        return userRepository.findById(id).orElseGet(null);
    }

    public List<User> findAll() {
        return userRepository.findAll();
    }

    @Override
    public User save(UserRegistrationDto userRequest) {
        User u = new User();
        u.setUsername(userRequest.getUsername());
        u.setPassword(passwordEncoder.encode(userRequest.getPassword()));
        u.setEmail(userRequest.getEmail());
        u.setFirstName(userRequest.getFirstName());
        u.setLastName(userRequest.getLastName());

        Role role = roleService.findByName("ROLE_" + userRequest.getRoleName().toUpperCase());
        u.setRole(role);

        User savedUser = userRepository.save(u);

        neo4jUserService.createUser(savedUser.getUsername());

        return savedUser;
    }

    @Override
    public User save(User user) {
        return userRepository.save(user);
    }

    @Override
    public User update(Long id, UserDTO userDto) {
        User user = userRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("User not found with id: " + id));

        user.setFirstName(userDto.getFirstName());
        user.setLastName(userDto.getLastName());
        user.setProfilePicture(userDto.getProfilePicture());
        user.setBiography(userDto.getBiography());
        user.setMotto(userDto.getMotto());
        user.setEmail(userDto.getEmail());

        return userRepository.save(user);
    }

    @Override
    @org.springframework.transaction.annotation.Transactional
    public User toggleUserBlockAndSync(Long id, boolean block) {
        User user = userRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("User not found with id: " + id));

        user.setBlocked(block);
        return userRepository.save(user);
    }
}
